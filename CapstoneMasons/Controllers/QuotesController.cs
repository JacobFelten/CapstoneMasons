using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.ViewModels;

namespace CapstoneMasons.Controllers
{
    public class QuotesController : Controller
    {
        IQuoteRepository repo;
        IFormulaRepository repoF;

        public QuotesController(IQuoteRepository repository, IFormulaRepository repositoryF)
        {
            repo = repository;
            repoF = repositoryF;
        }

        // GET: Quotes
        public async Task<IActionResult> Index()
        {
            return View(await repo.Quotes);
        }

        // GET: Quotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // GET: Quotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuoteID,Name,OrderNum,DateQuoted,PickedUp,Open")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                await repo.AddQuoteAsync(quote);
                return RedirectToAction(nameof(Index));
            }
            return View(quote);
        }

        public async Task<IActionResult> ReviewQuote(Quote q)
        {
            ReviewQuote rQ = new ReviewQuote();
            rQ.QuoteID = q.QuoteID; //done
            rQ.Name = q.Name; //done
            rQ.OrderNum = q.OrderNum; //done

            //After filling the shapes with instructions the shapes are sorted by bar size
            //then by shape length so the instructions make sense.
            rQ.Shapes = await CreateShapesAsync(q);
            IOrderedEnumerable<ReviewShape> orderedEnumerable = rQ.Shapes.OrderBy(s => s.BarSize).ThenByDescending(s => s.CutLength);
            List<ReviewShape> rSList = new List<ReviewShape>();
            foreach (ReviewShape rS in orderedEnumerable)
                rSList.Add(rS);
            rQ.Shapes = rSList; // TEMP!!! done

            rQ.BarsUsed = CalculateBarsUsed(rSList, q); //done

            rQ.TotalCost = CalculateTotalCost(rQ.BarsUsed); //done
            rQ.SetUpCharge = CalculateSetUp(rQ.TotalCost, q); //Done
            rQ.TotalCost += rQ.SetUpCharge; //done

            rQ.FinalRemnants = CalculateFinalRemnants(rSList); //done

            return View(rQ);
        }

        // GET: Quotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuoteID,Name,OrderNum,DateQuoted,PickedUp,Open")] Quote newQuote)
        {
            if (id != newQuote.QuoteID)
            {
                return NotFound();
            }

            Quote oldQuote = null;

            if (ModelState.IsValid)
            {
                try
                {
                    oldQuote = await repo.GetQuoteByIdAsync(newQuote.QuoteID);
                    await repo.UpdateQuoteAsync(oldQuote, newQuote);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuoteExists(oldQuote.QuoteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(oldQuote);
        }

        // GET: Quotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quote = await repo.GetQuoteByIdAsync(id);
            await repo.DeleteQuoteAsync(quote);
            return RedirectToAction(nameof(Index));
        }

        private bool QuoteExists(int id)
        {
            var quotes = (IQueryable<Quote>)repo.GetAllQuotesAsync();
            return quotes.Any(e => e.QuoteID == id);
        }

        #region Methods for ReviewQuote

        //Creates a ReviewShape for every shape in the quote. This method adds the simple
        //data before calling FillShapes which calculates remnants.
        private async Task<List<ReviewShape>> CreateShapesAsync(Quote q)
        {
            List<ReviewShape> rSList = new List<ReviewShape>();
            foreach (Shape s in q.Shapes)
            {
                ReviewShape rS = new ReviewShape();
                rS.ShapeID = s.ShapeID;
                rS.Qty = s.Qty;
                rS.BarSize = s.BarSize;
                rS.CutLength = await CalculateShapeLengthAsync(s);
                rS.Legs = await CreateLegsAsync(s);
                rSList.Add(rS);
            }
            rSList = FillShapes(rSList);
            return rSList;
        }

        private List<ReviewShape> FillShapes(List<ReviewShape> rSList)
        {
            //This seperates the shape list into lists that are just of one bar size
            List<List<ReviewShape>> listList = new List<List<ReviewShape>>();
            listList.Add(GetShapesByBar(rSList, 3));
            listList.Add(GetShapesByBar(rSList, 4));
            listList.Add(GetShapesByBar(rSList, 5));
            listList.Add(GetShapesByBar(rSList, 6));

            for (int i = 0; i < listList.Count; i++)
                if (listList[i].Count > 0)
                    listList[i] = FillShapeOfBarSize(listList[i]);

            List<ReviewShape> result = new List<ReviewShape>();
            foreach (List<ReviewShape> list in listList)
                foreach (ReviewShape rS in list)
                    result.Add(rS);

            return result;
        }

        private List<ReviewShape> GetShapesByBar(List<ReviewShape> rSList, int barNum)
        {
            List<ReviewShape> result = new List<ReviewShape>();
            foreach (ReviewShape rS in rSList)
                if (rS.BarSize == barNum)
                    result.Add(rS);
            return result;
        }

        //---------------------------------------------------------------------
        //THIS METHOD IS NOT FINISHED BUT IS ENOUGHT FOR THE FIRST TEST EXAMPLE
        //---------------------------------------------------------------------
        private List<ReviewShape> FillShapeOfBarSize(List<ReviewShape> rSList)
        {
            //Sorts the list of shapes by length
            rSList.Sort((a, b) => b.CutLength.CompareTo(a.CutLength));
            for (int i = 0; i < rSList.Count; i++)
            {
                if (i == 0) //First Shape
                {
                    //The first shape is the longest so it won't use remnants
                    rSList[i] = FillShapeNotUsingRemnants(rSList[i]);
                }
                else //Not first
                {
                    List<Remnant> remnants = GetRemnants(rSList);
                    List<CutInstruction> cIList = new List<CutInstruction>();
                    List<Remnant> rList = new List<Remnant>();
                    int shapesLeft = rSList[i].Qty;
                    bool useRemnants = false;
                    remnants.Sort((a, b) => a.Length.CompareTo(b.Length));
                    for (int rIndex = 0; rIndex < remnants.Count; rIndex++)
                    {
                        //While looping through all the remnants this checks if the remnant
                        //is long enough to make the shape from.
                        if (rSList[i].CutLength < remnants[rIndex].Length)
                        {
                            //The code below is very similar to FillShapeNotUsingRemnants but
                            //uses the dimensions of a remnant instead of a full 240" bar
                            remnants[rIndex].UsedAgain = true;
                            useRemnants = true;
                            decimal remnant;
                            int perBar = GetShapesPerBar(rSList[i].CutLength, remnants[rIndex].Length, out remnant);
                            int forQty = 0;
                            //This IF sees if the qty of shapes needed is more than there
                            //are remnants of this size.
                            //
                            //THE ELSE OF THIS IF STATEMENT STILL NEEDS TO BE CODED
                            if (shapesLeft > perBar * remnants[rIndex].Qty)
                            {
                                forQty = remnants[rIndex].Qty;
                                rList.Add(new Remnant
                                {
                                    Length = remnant,
                                    Qty = remnants[rIndex].Qty,
                                    UsedAgain = false
                                });
                                cIList.Add(new CutInstruction
                                {
                                    CutQty = perBar,
                                    PerLength = remnants[rIndex].Length,
                                    PerType = "Remnant",
                                    ForQty = forQty
                                });
                                decimal finalRemnant;
                                int finalPerBar;
                                if (GetShapesPerFinalBar(shapesLeft, rSList[i].CutLength, remnants[rIndex].Length, out finalRemnant, out finalPerBar))
                                {
                                    rList.Add(new Remnant
                                    {
                                        Length = finalRemnant,
                                        Qty = 1,
                                        UsedAgain = false
                                    });
                                    cIList.Add(new CutInstruction
                                    {
                                        CutQty = finalPerBar,
                                        PerLength = remnants[rIndex].Length,
                                        PerType = "Remnant",
                                        ForQty = 1
                                    });
                                }

                                ReviewShape tempRS = new ReviewShape
                                {
                                    Qty = rSList[i].Qty - (remnants[rIndex].Qty * perBar),
                                    CutLength = rSList[i].CutLength
                                };

                                //This is for the remaining shapes that there weren't enough
                                //remnants for.
                                tempRS = FillShapeNotUsingRemnants(tempRS);

                                foreach (CutInstruction cI in tempRS.Instructions)
                                    cIList.Add(cI);
                                foreach (Remnant r in tempRS.Remnants)
                                    rList.Add(r);

                                rSList[i].NumOfBars = tempRS.NumOfBars;
                                rIndex = remnants.Count; //Force end the loop
                            }
                        }
                    }
                    if (!useRemnants)
                        rSList[i] = FillShapeNotUsingRemnants(rSList[i]);
                    else
                    {
                        rSList[i].Instructions = cIList;
                        rSList[i].Remnants = rList;
                    }
                }
            }
            return rSList;
        }

        //Returns all the remnants that have been added to a list of shapes
        private List<Remnant> GetRemnants(List<ReviewShape> rSList)
        {
            List<Remnant> result = new List<Remnant>();
            foreach (ReviewShape rS in rSList)
            {
                if (rS.Remnants != null)
                    foreach (Remnant r in rS.Remnants)
                        result.Add(r);
            }
            return result;
        }

        private ReviewShape FillShapeNotUsingRemnants(ReviewShape rS)
        {
            List<Remnant> rList = new List<Remnant>();
            List<CutInstruction> cIList = new List<CutInstruction>();
            decimal remnant;
            int perBar = GetShapesPerBar(rS.CutLength, 240, out remnant);
            rList.Add(new Remnant
            {
                Length = remnant,
                Qty = rS.Qty / perBar,
                UsedAgain = false
            });
            cIList.Add(new CutInstruction
            {
                CutQty = perBar,
                PerLength = 240,
                PerType = "Bar",
                ForQty = rS.Qty / perBar
            });
            decimal finalRemnant;
            int finalPerBar;
            //The code before this IF calulates remnants and instructions from cutting from
            //240" bars. The code inside the IF is for situations when the number of shapes
            //you get per a 240" bar does not evenly divide into the qty of shapes needed for
            //the quote. An example would be someone needs 7 bars that are 72" long. The seventh
            //shape cut would leave a different sized remnant because you get 3 shapes per bar,
            //and 3 doesn't evenly divide into 7.
            if (GetShapesPerFinalBar(rS.Qty, rS.CutLength, 240, out finalRemnant, out finalPerBar))
            {
                rList.Add(new Remnant
                {
                    Length = finalRemnant,
                    Qty = 1,
                    UsedAgain = false
                });
                cIList.Add(new CutInstruction
                {
                    CutQty = finalPerBar,
                    PerLength = 240,
                    PerType = "Bar",
                    ForQty = 1
                });
            }
            rS.NumOfBars = (int)Math.Ceiling((decimal)rS.Qty / perBar);
            rS.Instructions = cIList;
            rS.Remnants = rList;

            return rS;
        }

        //Returns both the shapes per bar and the length of the remnant at the end of each bar
        private int GetShapesPerBar(decimal shapeLength, decimal barLength, out decimal remnant)
        {
            remnant = barLength % shapeLength;
            return (int)Math.Floor(barLength / shapeLength);
        }

        //Returns true if shapes per bar doesn't evenly divide into the qty needed.
        //Also returns the shapes per last bar (the 7th shape from the example in a previous
        //comment) and the remnant left by the last bar cut into.
        private bool GetShapesPerFinalBar(int shapeQty, decimal shapeLength, decimal barLength, out decimal remnant, out int perBar)
        {
            perBar = shapeQty % ((int)Math.Floor(barLength / shapeLength));
            if (perBar == 0)
            {
                remnant = 0;
                return false;
            }
            else
            {
                remnant = barLength - (shapeLength * perBar);
                return true;
            }
        }

        private async Task<List<ReviewLeg>> CreateLegsAsync(Shape s)
        {
            List<ReviewLeg> rLList = new List<ReviewLeg>();
            foreach (Leg l in s.Legs)
            {
                ReviewLeg rL = new ReviewLeg();
                rL.Length = l.Length;
                rL.Degree = l.Degree;
                Formula f = await GetFormulaByLegAsync(l, s.BarSize);
                if (f != null)
                {
                    rL.Mandrel = f.Mandrel.Name;
                    rL.PinNumber = f.PinNumber;
                    rL.InGained = f.InGained;
                }
                else
                {
                    rL.Mandrel = null;
                    rL.PinNumber = 0;
                    rL.InGained = 0;
                }
                rLList.Add(rL);
            }
            return rLList;
        }

        private List<RemnantList> CalculateFinalRemnants(List<ReviewShape> rSList)
        {
            List<RemnantList> result = new List<RemnantList>();
            List<List<ReviewShape>> listList = new List<List<ReviewShape>>();
            listList.Add(GetShapesByBar(rSList, 3));
            listList.Add(GetShapesByBar(rSList, 4));
            listList.Add(GetShapesByBar(rSList, 5));
            listList.Add(GetShapesByBar(rSList, 6));

            for (int i = 0; i < listList.Count; i++)
                if (listList[i].Count > 0)
                {
                    RemnantList rL = FillRemnantList(listList[i]);
                    rL.BarSize = listList[i][0].BarSize;
                    result.Add(rL);
                }

            return result;
        }

        //This gets all the remnants that are left over from the quote by checking
        //their UsedAgain value.
        private RemnantList FillRemnantList(List<ReviewShape> rSList)
        {
            RemnantList result = new RemnantList();
            result.Remnants = new List<Remnant>();
            for (int i = 0; i < rSList.Count; i++)
            {
                for (int j = 0; j < rSList[i].Remnants.Count; j++)
                {
                    if (!rSList[i].Remnants[j].UsedAgain)
                    {
                        result.Remnants.Add(rSList[i].Remnants[j]);
                    }
                }
            }
            return result;
        }

        //Finds the setup charge stored in the quote's cost list
        private decimal CalculateSetUp(decimal total, Quote q)
        {
            decimal setup = 0;
            decimal setupMin = 0;

            foreach (Cost c in q.Costs)
            {
                if (c.Name == "Setup")
                    setup = c.Price;
                else if (c.Name == "Setup Min")
                    setupMin = c.Price;
            }

            if (total < setupMin)
                return setup;
            else
                return 0;
        }

        private List<UsedBar> CalculateBarsUsed(List<ReviewShape> rSList, Quote q)
        {
            List<UsedBar> result = new List<UsedBar>();

            List<List<ReviewShape>> listList = new List<List<ReviewShape>>();
            listList.Add(GetShapesByBar(rSList, 3));
            listList.Add(GetShapesByBar(rSList, 4));
            listList.Add(GetShapesByBar(rSList, 5));
            listList.Add(GetShapesByBar(rSList, 6));

            for (int i = 0; i < listList.Count; i++)
                if (listList[i].Count > 0)
                    result.Add(CreateUsedBar(listList[i], q));

            return result;
        }

        //This gets the number of bars, cuts, and bends that are in a list of ReviewShapes.
        //A quote is passed in because it has the info on what each of those costs in 
        //its list of costs.
        private UsedBar CreateUsedBar(List<ReviewShape> rSList, Quote q)
        {
            UsedBar result = new UsedBar();

            result.BarSize = rSList[0].BarSize;

            int numOfBars = 0;
            int numOfCuts = 0;
            int numOfBends = 0;

            foreach (ReviewShape rS in rSList)
            {
                numOfBars += rS.NumOfBars;
                numOfCuts += rS.Qty;
                numOfBends += rS.Qty * (rS.Legs.Count - 1);
            }

            result.NumOfBars = numOfBars;
            result.NumOfCuts = numOfCuts;
            result.NumOfBends = numOfBends;

            foreach (Cost c in q.Costs)
            {
                if (c.Name == result.BarSize + " Bar")
                    result.BarCost = c.Price;
                else if (c.Name == result.BarSize + " Cut")
                    result.CutCost = c.Price;
                else if (c.Name == result.BarSize + " Bend")
                    result.BendCost = c.Price;
            }

            return result;
        }

        //This calculates the total cost before setup charge
        private decimal CalculateTotalCost(List<UsedBar> usedBars)
        {
            decimal totalCost = 0;
            foreach (UsedBar u in usedBars)
            {
                totalCost += u.NumOfBars * u.BarCost;
                totalCost += u.NumOfCuts * u.CutCost;
                totalCost += u.NumOfBends * u.BendCost;
            }
            return totalCost;
        }

        //The length is the legs minus the inches gained found in the matching formula
        private async Task<decimal> CalculateShapeLengthAsync(Shape s)
        {
            decimal length = 0;

            for (int i = 0; i < s.Legs.Count - 1; i++)
            {
                Leg l = s.Legs[i];
                length += l.Length;
                Formula f = await GetFormulaByLegAsync(l, s.BarSize);
                length -= f.InGained;
            }
            length += s.Legs[s.Legs.Count - 1].Length;

            return length;
        }

        private async Task<Formula> GetFormulaByLegAsync(Leg l, int barSize)
        {
            foreach (Formula f in await repoF.Formulas)
                if (l.Degree == f.Degree &&
                    l.Mandrel == f.Mandrel &&
                    barSize == f.BarSize)
                    return f;
            return null;
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BarCost(List<Cost> Costs)
        {
            var costsQuote = await repo.BarCosts;//get costs from first quote? seeded?
            if (ModelState.IsValid)
            {
                for (int i = 0; i < 15; i++)//could be foreach too 
                {
                    if (costsQuote.FirstOrDefault(c => c.CostID == i).Price != Costs[i].Price)
                    {
                        await repo.UpdateCostAsync(costsQuote.FirstOrDefault(c => c.CostID == i), Costs[i]);
                    }
                }
                costsQuote = await repo.BarCosts;
                return View(costsQuote.ToList());
            }
            return View(costsQuote.ToList());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Quote> UpdatePrices(Quote quote)
        {
            quote.Costs.Clear();
            var costsQuote = await repo.BarCosts;//get costs from first quote? seeded?
            var sumLegs = 0m;
            var total = 0m;

            foreach (Shape shape in quote.Shapes)
            {
                var barCost = costsQuote.FirstOrDefault(c => c.Name == shape.BarSize.ToString() + " Bar");
                if (!quote.Costs.Contains(barCost))//if it doesnt contain add cost
                    quote.Costs.Add(barCost);//Adding Cost per Bar
                total += barCost.Price * shape.Qty;

                foreach (Leg leg in shape.Legs)
                {
                    sumLegs += leg.Length;
                }
                if (sumLegs != 240)
                {
                    var cutCost = costsQuote.FirstOrDefault(c => c.Name == shape.BarSize.ToString() + " Cut");
                    if (!quote.Costs.Contains(cutCost))
                        quote.Costs.Add(cutCost);//Adding Cost per cut
                    //total += costsQuote.Costs.Find(c => c.Name == shape.BarSize.ToString() + "Cut").Price*quote.; //add cut prices later?
                }

                for (int i = 0; i < shape.LegCount - 1; i++)
                {
                    var bendCost = costsQuote.FirstOrDefault(c => c.Name == shape.BarSize.ToString() + " Bend");
                    if (!quote.Costs.Contains(bendCost))
                        quote.Costs.Add(bendCost);//Adding Cost per bend
                    total += bendCost.Price;
                }

            }
            if (total < costsQuote.FirstOrDefault(c => c.Name == "Setup Min").Price)
            {
                quote.Costs.Add(costsQuote.FirstOrDefault(c => c.Name == "Setup"));//BarSize3Cut
                total += costsQuote.FirstOrDefault(c => c.Name == "Setup").Price;
            }

            return quote;
        }
    }
}
