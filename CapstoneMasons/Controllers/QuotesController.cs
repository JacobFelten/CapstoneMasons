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
            OpenQuote open = new OpenQuote();
            Quote q = await DummyQuote();
            if((await repo.Quotes).Count == 0)
            {
                await repo.AddQuoteAsync(q);
            }
            open.Quotes = await repo.Quotes;
            ReviewQuote rvQ = await FillReviewQuote(open.Quotes[0]);
            open.TotalCost = rvQ.TotalCost;
            return View(open);
        }

        //[HttpPost]
        //public async Task<IActionResult> Search()
        //{

        //    return View("Index",);
        //}

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
            if ((await repo.Quotes).Count == 0)//THIS IS ONLY FOR TESTING
            {
                await repo.AddQuoteAsync(q);
            }
            q = await repo.GetQuoteByIdAsync(5);//THIS IS ONLY FOR TESTING
            

            ReviewQuote rQ = new ReviewQuote();
            rQ.QuoteID = q.QuoteID; //done
            rQ.Name = q.Name; //done
            rQ.OrderNum = q.OrderNum; //done
            rQ.AddSetup = q.AddSetup;
            rQ.Discount = q.Discount;

            //After filling the shapes with instructions the shapes are sorted by bar size
            //then by shape length so the instructions make sense.
            rQ.Shapes = await CreateShapesAsync(q);
            IOrderedEnumerable<ReviewShape> orderedEnumerable = rQ.Shapes.OrderBy(s => s.BarSize).ThenByDescending(s => s.CutLength);
            List<ReviewShape> rSList = new List<ReviewShape>();
            foreach (ReviewShape rS in orderedEnumerable)
                rSList.Add(rS);
            rQ.Shapes = rSList; // done

            rQ.BarsUsed = CalculateBarsUsed(rSList, q); //done

            rQ.TotalCost = CalculateTotalCost(rQ.BarsUsed); //done
            rQ.SetUpCharge = CalculateSetUp(q, rQ.BarsUsed); //Done
            rQ.TotalCost += rQ.SetUpCharge; //done
            rQ.TotalCost -= rQ.Discount; //done

            rQ.FinalRemnants = CalculateFinalRemnants(rSList); //done

            return View(rQ);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewQuote(int quoteID, string name, string orderNumber, int discount, string setup)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);
            await repo.UpdateQuoteSimpleAsync(q, "Name", name);
            await repo.UpdateQuoteSimpleAsync(q, "OrderNum", orderNumber);
            await repo.UpdateQuoteSimpleAsync(q, "Discount", discount.ToString());
            await repo.UpdateQuoteSimpleAsync(q, "AddSetup", setup);
            return await ReviewQuote(q);
        }

        #region So Jacob doesn't have to keep scrolling past these
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
        #endregion

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
                rS.CutLength = await CalculateShapeLengthAsync(s); //Here's where Jeff jumps in
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

            Remove0LengthRemnants(result);
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

        private List<ReviewShape> FillShapeOfBarSize(List<ReviewShape> rSList)
        {
            //Sorts the list of shapes by length
            rSList.Sort((a, b) => b.CutLength.CompareTo(a.CutLength));
            for (int i = 0; i < rSList.Count; i++)
            {
                int shapesLeft = rSList[i].Qty;
                List<CutInstruction> cIList = new List<CutInstruction>();
                List<Remnant> rList = new List<Remnant>();
                if (i == 0) //First Shape
                {
                    //The first shape is the longest so it won't use remnants
                    rSList[i] = FillShapeNotUsingRemnants(rSList[i]);
                    shapesLeft = 0;
                }
                else //Not first
                {
                    List<Remnant> remnants = GetRemnants(rSList);
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
                            //are remnants of this size OR if the qty of shapes needed can be
                            //fulfilled with just one remnant.
                            if (shapesLeft > (perBar * remnants[rIndex].Qty) - perBar)
                            {
                                forQty = remnants[rIndex].Qty;
                                if (perBar <= shapesLeft)
                                {
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
                                    shapesLeft -= perBar * forQty;
                                }
                                decimal finalRemnant;
                                int finalPerBar;
                                if (GetShapesPerFinalBar(shapesLeft, rSList[i].CutLength, remnants[rIndex].Length, out finalRemnant, out finalPerBar) &&
                                    forQty % perBar != 0)
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
                                    shapesLeft -= finalPerBar;
                                }
                            }
                            else
                            {
                                int remainingRems = remnants[rIndex].Qty;
                                forQty = shapesLeft / perBar;
                                if (perBar <= shapesLeft)
                                {
                                    rList.Add(new Remnant
                                    {
                                        Length = remnant,
                                        Qty = forQty,
                                        UsedAgain = false
                                    });
                                    cIList.Add(new CutInstruction
                                    {
                                        CutQty = perBar,
                                        PerLength = remnants[rIndex].Length,
                                        PerType = "Remnant",
                                        ForQty = forQty
                                    });
                                    shapesLeft -= perBar * forQty;
                                    remainingRems -= forQty;
                                }
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
                                    shapesLeft -= finalPerBar;
                                    remainingRems -= 1;
                                }
                                rList.Add(new Remnant
                                {
                                    Length = remnants[rIndex].Length,
                                    Qty = remainingRems,
                                    UsedAgain = false
                                });
                            }

                            if (shapesLeft == 0)
                                rIndex = remnants.Count; //Force end the loop
                            else if (shapesLeft < 0)
                                throw new Exception();
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

                if (shapesLeft > 0)
                {
                    ReviewShape tempRS = new ReviewShape
                    {
                        Qty = shapesLeft,
                        CutLength = rSList[i].CutLength
                    };

                    //This is for the remaining shapes that there weren't enough
                    //remnants for.
                    if (tempRS.Qty > 0)
                    {
                        tempRS = FillShapeNotUsingRemnants(tempRS);

                        foreach (CutInstruction cI in tempRS.Instructions)
                            cIList.Add(cI);
                        foreach (Remnant r in tempRS.Remnants)
                            rList.Add(r);

                        rSList[i].NumOfBars = tempRS.NumOfBars;
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
                        if (!r.UsedAgain)
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
            if (perBar <= rS.Qty)
            {
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
            }
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

        private void Remove0LengthRemnants(List<ReviewShape> rSList)
        {
            foreach (ReviewShape rS in rSList)
            {
                for (int i = 0; i < rS.Remnants.Count; i++)
                {
                    if (rS.Remnants[i].Length == 0)
                    {
                        rS.Remnants.RemoveAt(i);
                        i--;
                    }
                }
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
                    rL.PinNumber = "";
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
                    CombineRemnants(rL);
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
                        result.Remnants.Add(new Remnant
                        {
                            Qty = rSList[i].Remnants[j].Qty,
                            Length = rSList[i].Remnants[j].Length,
                            UsedAgain = rSList[i].Remnants[j].UsedAgain
                        });
                    }
                }
            }
            return result;
        }

        private void CombineRemnants(RemnantList rL)
        {
            rL.Remnants.Sort((a, b) => a.Length.CompareTo(b.Length));
            for (int i = 1; i < rL.Remnants.Count; i++)
            {
                if (rL.Remnants[i].Length == rL.Remnants[i - 1].Length)
                {
                    rL.Remnants[i - 1].Qty += rL.Remnants[i].Qty;
                    rL.Remnants.RemoveAt(i);
                    i--;
                }
            }
        }

        //Finds the setup charge stored in the quote's cost list
        private decimal CalculateSetUp(Quote q, List<UsedBar> usedBars)
        {
            decimal setup = 0;
            int totalCutsBends = 0;

            foreach (Cost c in q.Costs)
            {
                if (c.Name == "Setup")
                    setup = c.Price;
            }

            foreach (UsedBar uB in usedBars)
            {
                totalCutsBends += uB.NumOfBends;
                totalCutsBends += uB.NumOfCuts;
            }

            if (totalCutsBends < 100)
            {
                if (q.AddSetup != false)
                    return setup;
                else
                    return 0;
            }
            else
            {
                if (q.AddSetup != true)
                    return 0;
                else
                    return setup;
            }
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

        private async Task<ReviewQuote> FillReviewQuote(Quote q) 
        {
            ReviewQuote rQ = new ReviewQuote();
            rQ.QuoteID = q.QuoteID; //done
            rQ.Name = q.Name; //done
            rQ.OrderNum = q.OrderNum; //done
            rQ.AddSetup = q.AddSetup;
            rQ.Discount = q.Discount;

            //After filling the shapes with instructions the shapes are sorted by bar size
            //then by shape length so the instructions make sense.
            rQ.Shapes = await CreateShapesAsync(q);
            IOrderedEnumerable<ReviewShape> orderedEnumerable = rQ.Shapes.OrderBy(s => s.BarSize).ThenByDescending(s => s.CutLength);
            List<ReviewShape> rSList = new List<ReviewShape>();
            foreach (ReviewShape rS in orderedEnumerable)
                rSList.Add(rS);
            rQ.Shapes = rSList; // done

            rQ.BarsUsed = CalculateBarsUsed(rSList, q); //done

            rQ.TotalCost = CalculateTotalCost(rQ.BarsUsed); //done
            rQ.SetUpCharge = CalculateSetUp(q, rQ.BarsUsed); //Done
            rQ.TotalCost += rQ.SetUpCharge; //done
            rQ.TotalCost -= rQ.Discount; //done

            rQ.FinalRemnants = CalculateFinalRemnants(rSList); //done

            return rQ;
        }

        #region Jacob's Dummy thicc data for testing ma view
        private async Task<Quote> DummyQuote()
        {
            var mandrel1 = await repoF.GetMandrelByIdAsync(2);
            var mandrel2 = await repoF.GetMandrelByIdAsync(3);
            var leg1 = new Leg
            {
                Length = 36,
                Degree = 90,
                Mandrel = mandrel2
            };
            var leg2 = new Leg
            {
                Length = 36,
                Degree = 0,
                Mandrel = null
            };
            var leg3 = new Leg
            {
                Length = 12,
                Degree = 90,
                Mandrel = mandrel2
            };
            var leg4 = new Leg
            {
                Length = 12,
                Degree = 0,
                Mandrel = null
            };
            var leg5 = new Leg
            {
                Length = 30,
                Degree = 90,
                Mandrel = mandrel1
            };
            var leg6 = new Leg
            {
                Length = 18,
                Degree = 90,
                Mandrel = mandrel1
            };
            var leg7 = new Leg
            {
                Length = 30,
                Degree = 0,
                Mandrel = null
            };
            var shape2 = new Shape
            {
                BarSize = 5,
                LegCount = 2,
                Legs = { leg1, leg2 },
                Qty = 30,
                NumCompleted = 27
            };
            var shape3 = new Shape
            {
                BarSize = 5,
                LegCount = 2,
                Legs = { leg3, leg4 },
                Qty = 60,
                NumCompleted = 5
            };
            var shape4 = new Shape
            {
                BarSize = 4,
                LegCount = 3,
                Legs = { leg5, leg6, leg7 },
                Qty = 40,
                NumCompleted = 3
            };
            var cost1 = new Cost
            {
                Name = "4 Bar",
                Price = 10,
                LastChanged = new DateTime()
            };
            var cost2 = new Cost
            {
                Name = "4 Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            var cost3 = new Cost
            {
                Name = "4 Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            var cost4 = new Cost
            {
                Name = "5 Bar",
                Price = 15,
                LastChanged = new DateTime()
            };
            var cost5 = new Cost
            {
                Name = "5 Cut",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            var cost6 = new Cost
            {
                Name = "5 Bend",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            var cost7 = new Cost
            {
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            var quote2 = new Quote
            {
                Name = "Bob's Concrete",
                OrderNum = "123456",
                Shapes = { shape2, shape3, shape4 },
                Costs = { cost1, cost2, cost3, cost4, cost5, cost6, cost7 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            return quote2;
        }
        
        private Quote DummyQuote2()
        {
            var leg1 = new Leg
            {
                LegID = 1,
                Length = 72
            };
            var shape1 = new Shape
            {
                ShapeID = 1,
                BarSize = 6,
                LegCount = 1,
                Legs = { leg1 },
                Qty = 25,
                NumCompleted = 0,
            };
            var cost1 = new Cost
            {
                CostID = 1,
                Name = "6 Bar",
                Price = 20,
                LastChanged = new DateTime()
            };
            var cost2 = new Cost
            {
                CostID = 2,
                Name = "6 Cut",
                Price = 0.40m,
                LastChanged = new DateTime()
            };
            var cost3 = new Cost
            {
                CostID = 3,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            var quote = new Quote
            {
                QuoteID = 1,
                Name = "Billy's Concrete",
                OrderNum = "654312",
                Shapes = { shape1 },
                Costs = { cost1, cost2, cost3 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            return quote;
        }

        private Quote DummyQuote3()
        {
            var leg1 = new Leg
            {
                LegID = 1,
                Length = 120
            };
            var shape1 = new Shape
            {
                ShapeID = 1,
                BarSize = 6,
                LegCount = 1,
                Legs = { leg1 },
                Qty = 1,
                NumCompleted = 0,
            };
            var leg2 = new Leg
            {
                LegID = 2,
                Length = 60
            };
            var shape2 = new Shape
            {
                ShapeID = 2,
                BarSize = 6,
                LegCount = 1,
                Legs = { leg2 },
                Qty = 1,
                NumCompleted = 0,
            };
            var leg3 = new Leg
            {
                LegID = 3,
                Length = 24
            };
            var shape3 = new Shape
            {
                ShapeID = 3,
                BarSize = 6,
                LegCount = 1,
                Legs = { leg3 },
                Qty = 1,
                NumCompleted = 0,
            };
            var leg4 = new Leg
            {
                LegID = 4,
                Length = 12
            };
            var shape4 = new Shape
            {
                ShapeID = 4,
                BarSize = 6,
                LegCount = 1,
                Legs = { leg4 },
                Qty = 1,
                NumCompleted = 0,
            };
            var cost1 = new Cost
            {
                CostID = 1,
                Name = "6 Bar",
                Price = 20,
                LastChanged = new DateTime()
            };
            var cost2 = new Cost
            {
                CostID = 2,
                Name = "6 Cut",
                Price = 0.40m,
                LastChanged = new DateTime()
            };
            var cost3 = new Cost
            {
                CostID = 3,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            var quote = new Quote
            {
                QuoteID = 1,
                Name = "Jill's Concrete",
                OrderNum = "987654",
                Shapes = { shape1, shape2, shape3, shape4 },
                Costs = { cost1, cost2, cost3 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            return quote;
        }

        private async Task<Quote> DummyQuote4()
        {
            var mandrel1 = await repoF.GetMandrelByIdAsync(2);
            var mandrel2 = await repoF.GetMandrelByIdAsync(3);
            var leg1 = new Leg
            {
                Length = 60,
                Degree = 45,
                Mandrel = mandrel1
            };
            var leg2 = new Leg
            {
                Length = 25,
                Degree = 90,
                Mandrel = mandrel1
            };
            var leg3 = new Leg
            {
                Length = 120,
                Degree = 0,
                Mandrel = null
            };
            var shape1 = new Shape
            {
                BarSize = 3,
                LegCount = 3,
                Legs = { leg1, leg2, leg3 },
                Qty = 3,
                NumCompleted = 0,
            };
            var leg4 = new Leg
            {
                Length = 24,
                Degree = 0,
                Mandrel = null
            };
            var shape2 = new Shape
            {
                BarSize = 3,
                LegCount = 3,
                Legs = { leg4 },
                Qty = 30,
                NumCompleted = 0,
            };
            var cost1 = new Cost
            {
                Name = "3 Bar",
                Price = 5,
                LastChanged = new DateTime()
            };
            var cost2 = new Cost
            {
                Name = "3 Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            var cost3 = new Cost
            {
                Name = "3 Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            var cost4 = new Cost
            {
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            var quote = new Quote
            {
                Name = "Jeff's Bucket 'O Bar",
                OrderNum = "420-69",
                Shapes = { shape1, shape2 },
                Costs = { cost1, cost2, cost3, cost4 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            return quote;
        }

        private Quote DummyQuote5()
        {
            var leg1 = new Leg
            {
                Length = 66
            };
            var shape1 = new Shape
            {
                BarSize = 6,
                LegCount = 1,
                Legs = { leg1 },
                Qty = 90,
                NumCompleted = 0,
            };
            var leg2 = new Leg
            {
                Length = 24
            };
            var shape2 = new Shape
            {
                BarSize = 6,
                LegCount = 1,
                Legs = { leg2 },
                Qty = 15,
                NumCompleted = 0,
            };
            var leg3 = new Leg
            {
                Length = 12
            };
            var shape3 = new Shape
            {
                BarSize = 6,
                LegCount = 1,
                Legs = { leg3 },
                Qty = 100,
                NumCompleted = 0,
            };
            var cost1 = new Cost
            {
                Name = "6 Bar",
                Price = 20,
                LastChanged = new DateTime()
            };
            var cost2 = new Cost
            {
                Name = "6 Cut",
                Price = 0.40m,
                LastChanged = new DateTime()
            };
            var cost3 = new Cost
            {
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            var quote = new Quote
            {
                Name = "Billy's Concrete",
                OrderNum = "654312",
                Shapes = { shape1, shape3, shape2 },
                Costs = { cost1, cost2, cost3 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            return quote;
        }
        #endregion
    }
}
