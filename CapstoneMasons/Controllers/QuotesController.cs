﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.ViewModels;
using CapstoneMasons.Logic_Models;
using CapstoneMasons.Infrastructure;
using Microsoft.AspNetCore.Connections.Features;
using Org.BouncyCastle.Asn1.X509;
using Microsoft.Extensions.Primitives;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

using Microsoft.AspNetCore.Authorization;


namespace CapstoneMasons.Controllers
{
    public class QuotesController : Controller
    {
        IQuoteRepository repo;
        IFormulaRepository repoF;
        ICostRepository repoC;
        IShapeRepository repoS;

        public QuotesController(IQuoteRepository repository, IFormulaRepository repositoryF, ICostRepository repositoryC, IShapeRepository repositoryS)
        {
            repo = repository;
            repoF = repositoryF;
            repoC = repositoryC;
            repoS = repositoryS;
        }

        // GET: Open Quotes
        public async Task<IActionResult> Index()
        {
            OpenQuote oq = new OpenQuote();
            foreach(Quote q in await repo.Quotes)
            {
                if (q.Open == true)
                {
                    ReviewQuote r = await FillReviewQuote(q);
                    bool stillYoung = false;
                    foreach(Cost c in await repoC.BarCosts)
                    {
                        if ((DateTime.Now - c.LastChanged).TotalDays < (DateTime.Now - q.Costs[0].LastChanged).TotalDays)
                        {
                            stillYoung = true;
                        }
                    }
                    if((DateTime.Now - q.Costs[0].LastChanged).TotalDays >= 14 && stillYoung)
                    {
                        r.Update = true;
                    }
                    oq.ReviewQuotes.Add(r);
                    oq.ReviewQuotes.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
            }
            return View(oq);
        }

        // Get: Closed Quotes
        public async Task<IActionResult> Closed()
        {
            OpenQuote oq = new OpenQuote();
            foreach (Quote q in await repo.Quotes)
            {
                if (q.Open == false)
                {
                    ReviewQuote r = await FillReviewQuote(q);
                    oq.ReviewQuotes.Add(r);
                    oq.ReviewQuotes.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
            }
            return View(oq);
        }

        [HttpPost]
        public async Task<IActionResult> SearchOpen(OpenQuote oq)
        {
            foreach(Quote q in await repo.Quotes)
            {
                if (q.Open == true)
                {
                    ReviewQuote r = await FillReviewQuote(q);
                    oq.ReviewQuotes.Add(r);
                }
            }
            if (oq.Sort2 != null)
            {
                List<ReviewQuote> rqList = new List<ReviewQuote>();
                IOrderedEnumerable<ReviewQuote> reviewQuotes;
                List<decimal> manpowerlist = new List<decimal>();
                switch (oq.Sort2)
                {
                    case "PickedUp":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            
                        }
                        break;
                    case "NotPickedUp":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                    case "ClosestToCompletion":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                    case "FarthestToCompletion":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                }
                oq.ReviewQuotes = rqList;
            }
            else
            {
                switch (oq.Sort)
                {
                    case "AtoZ":
                        oq.ReviewQuotes.Sort((a, b) => a.Name.CompareTo(b.Name));
                        break;
                    case "Cheapest":
                        oq.ReviewQuotes.Sort((a, b) => a.TotalCost.CompareTo(b.TotalCost));
                        break;
                    case "Most Expensive":
                        oq.ReviewQuotes.Sort((a, b) => b.TotalCost.CompareTo(a.TotalCost));
                        break;
                    case "Newest":
                        oq.ReviewQuotes.Sort((a, b) => b.DateQuoted.CompareTo(a.DateQuoted));
                        break;
                    case "Oldest":
                        oq.ReviewQuotes.Sort((a, b) => a.DateQuoted.CompareTo(b.DateQuoted));
                        break;
                }
            }
            if(oq.SearchBar != null)
            {
                List<int> quoteIndex = new List<int>();
                switch (oq.SearchBarSpecific)
                {
                    case "Name":
                        for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                        {
                            string smagastooble = oq.ReviewQuotes[i].Name;
                            smagastooble = smagastooble.ToLower().Trim().Replace("'", "");
                            if (!smagastooble.Contains(oq.SearchBar.ToLower().Trim().Replace("'", "")))
                            {
                                quoteIndex.Add(i);
                            }
                        }
                        quoteIndex.Sort((a, b) => b.CompareTo(a));
                        foreach (int i in quoteIndex)
                        {
                            oq.ReviewQuotes.RemoveAt(i);
                        }
                        break;

                    case "Order Num":
                        for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                        {
                            string smagastooble = oq.ReviewQuotes[i].OrderNum;
                            smagastooble = smagastooble.ToLower().Trim().Replace("'", "");
                            if (!smagastooble.Contains(oq.SearchBar.ToLower().Trim().Replace("'", "")))
                            {
                                quoteIndex.Add(i);
                            }
                        }
                        quoteIndex.Sort((a, b) => b.CompareTo(a));
                        foreach (int i in quoteIndex)
                        {
                            oq.ReviewQuotes.RemoveAt(i);
                        }
                        break;
                }
            }
            
            return View("Index", oq);
        }

        [HttpPost]
        public async Task<IActionResult> SearchClosed(OpenQuote oq)
        {
            foreach (Quote q in await repo.Quotes)
            {
                if (q.Open == false)
                {
                    ReviewQuote r = await FillReviewQuote(q);
                    oq.ReviewQuotes.Add(r);
                }
            }
            if (oq.Sort2 != null)
            {
                List<ReviewQuote> rqList = new List<ReviewQuote>();
                IOrderedEnumerable<ReviewQuote> reviewQuotes;
                List<decimal> manpowerlist = new List<decimal>();
                switch (oq.Sort2)
                {
                    case "PickedUp":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.PickedUp).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;

                        }
                        break;
                    case "NotPickedUp":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.PickedUp).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                    case "ClosestToCompletion":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderBy(rq => rq.QtyLeft).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                    case "FarthestToCompletion":
                        switch (oq.Sort)
                        {
                            case "AtoZ":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.Name);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Cheapest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Most Expensive":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenByDescending(rq => rq.TotalCost);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Newest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenByDescending(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                            case "Oldest":
                                for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                                {
                                    int wanted = 0;
                                    int made = 0;
                                    for (int j = 0; j < oq.ReviewQuotes[i].Shapes.Count; j++)
                                    {
                                        wanted += oq.ReviewQuotes[i].Shapes[j].Qty;
                                        made += oq.ReviewQuotes[i].Shapes[j].Completed;
                                    }
                                    oq.ReviewQuotes[i].QtyLeft = wanted - made;
                                }
                                reviewQuotes = oq.ReviewQuotes.OrderByDescending(rq => rq.QtyLeft).ThenBy(rq => rq.DateQuoted);
                                foreach (ReviewQuote rq in reviewQuotes)
                                {
                                    rqList.Add(rq);
                                }
                                break;
                        }
                        break;
                }
                oq.ReviewQuotes = rqList;
            }
            else
            {
                switch (oq.Sort)
                {
                    case "AtoZ":
                        oq.ReviewQuotes.Sort((a, b) => a.Name.CompareTo(b.Name));
                        break;
                    case "Cheapest":
                        oq.ReviewQuotes.Sort((a, b) => a.TotalCost.CompareTo(b.TotalCost));
                        break;
                    case "Most Expensive":
                        oq.ReviewQuotes.Sort((a, b) => b.TotalCost.CompareTo(a.TotalCost));
                        break;
                    case "Newest":
                        oq.ReviewQuotes.Sort((a, b) => b.DateQuoted.CompareTo(a.DateQuoted));
                        break;
                    case "Oldest":
                        oq.ReviewQuotes.Sort((a, b) => a.DateQuoted.CompareTo(b.DateQuoted));
                        break;
                }
            }
            if (oq.SearchBar != null)
            {
                List<int> quoteIndex = new List<int>();
                switch (oq.SearchBarSpecific)
                {
                    case "Name":
                        for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                        {
                            string smagastooble = oq.ReviewQuotes[i].Name;
                            smagastooble = smagastooble.ToLower().Trim().Replace("'", "");
                            if (!smagastooble.Contains(oq.SearchBar.ToLower().Trim().Replace("'", "")))
                            {
                                quoteIndex.Add(i);
                            }
                        }
                        quoteIndex.Sort((a, b) => b.CompareTo(a));
                        foreach (int i in quoteIndex)
                        {
                            oq.ReviewQuotes.RemoveAt(i);
                        }
                        break;

                    case "Order Num":
                        for (int i = 0; i < oq.ReviewQuotes.Count; i++)
                        {
                            string smagastooble = oq.ReviewQuotes[i].OrderNum;
                            smagastooble = smagastooble.ToLower().Trim().Replace("'", "");
                            if (!smagastooble.Contains(oq.SearchBar.ToLower().Trim().Replace("'", "")))
                            {
                                quoteIndex.Add(i);
                            }
                        }
                        quoteIndex.Sort((a, b) => b.CompareTo(a));
                        foreach (int i in quoteIndex)
                        {
                            oq.ReviewQuotes.RemoveAt(i);
                        }
                        break;
                }
            }

            return View("Closed", oq);
        }

        // GET: Quotes/Create
        public IActionResult Create(CreateQuote q)
        {
            //refactors CreateQuote to Quote

            Quote quote = new Quote()
            {
                Name = q.Name,
                OrderNum = q.OrderNum,
                UseFormulas = q.UseFormulas,
                Author = q.Author
            };
            for (var i = 0; i < q.ShapesCount; i++)
            {
                quote.Shapes.Add(new Shape());
                List<Leg> Legs = new List<Leg>();

                for (var j = 0; j < q.LegsInShapes[i]; j++)
                {
                    quote.Shapes[i].Legs.Add(new Leg()
                    {
                        SortOrder = j
                    });
                }
            }

            return View(quote);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Quote q)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {//Refactors  to Quote
                q.UseFormulas = false;
                for (var i = q.Shapes.Count-1; i > -1; i--)
                {

                    if (q.Shapes[i].Qty == -9 && q.Shapes[i].Legs[0].Length == -9)
                    {
                        q.Shapes.RemoveAt(i);
                    }
                    else
                    {
                        for (var j = 0; j < q.Shapes[i].Legs.Count - 1; j++)
                        {
                            q.Shapes[i].Legs[j].Mandrel = await repoF.GetMandrelByNameAsync(q.Shapes[i].Legs[j].Mandrel.Name);
                        }
                    }

                }
                
                //var quotes = await repo.Quotes;
                //int quoteId = quotes.Last().QuoteID + 1;
                //q.QuoteID = quoteId;
                //quotes.Add(q);
                //added to repo but not to the database
                var invalidLeg = false;
                var invalidShape = 0;
                foreach (Shape s in q.Shapes)
                {
                    foreach (Leg l in s.Legs)
                    {
                        if (l.Length > 240)
                        {
                            invalidLeg = true;
                            invalidShape = q.Shapes.IndexOf(s);
                            invalidShape++;
                        }
                    }
                }
                if (invalidLeg)
                {
                    ModelState.AddModelError(string.Empty, "A leg in shape "+ invalidShape.ToString()+ " and cannot be more than 240 inches");
                    return View("Create", q);
                }
                await repo.AddQuoteAsync(q);
                return RedirectToAction("ReviewQuote", new { quoteID = q.QuoteID });
                //return await ReviewQuote(q.QuoteID);
            }
            return View("Create", q);

        }

        [HttpPost]
        public IActionResult GoToReviewQuote(CreateQuote q)
        {            
            //var errors = ModelState.Values.SelectMany(v => v.Errors); testing
            if (ModelState.IsValid)
            {
                return Redirect(Url.Action(nameof(ReviewQuote), q));
            }
            return Redirect(Url.Action(nameof(Create), q));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(CreateQuote q)
        {
            if (ModelState.IsValid)
            {
                return Redirect(Url.Action(nameof(Create),q));
            }
            return Redirect(Url.Action("IndexPopUp", "Home",q));
        }

        public async Task<IActionResult> ReviewQuote(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);

            List<Formula> useFormulas = await CanUseFormulas(q);

            for (int i = 0; i < q.Shapes.Count; i++)
            {
                Shape s = q.Shapes[i];
                s.Legs.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
                string shapeNum = i < KnownObjects.NumberPrefix.Count ? KnownObjects.NumberPrefix[i] : (i + 1).ToString();
                decimal cutLength = 0;
                if (useFormulas.Count == 0)
                {
                    cutLength = await CalculateShapeLengthAsync(s); //Here's where Jeff jumps in
                }
                else
                {
                    cutLength = Calculations.Total_Shape_Length(s);
                    //Do jeff stuff
                    //pass in bar size as bar type, pass in legs as crude legs
                }
                if (cutLength > 240)
                {
                    ModelState.AddModelError(string.Empty, "The " + shapeNum + " shape cuts to longer than 240 inches.");
                    await repo.DeleteQuoteAsync(q);
                    return View("Create", q);
                }
            }

            string errorMessage = GenerateInvalidFormulaErrorMessage(useFormulas);
            if (errorMessage != "")
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            q.DateQuoted = TimeZoneInfo.ConvertTime(DateTime.Now,
                 TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));

            q = await UpdatePrices(q);

            q.Open = true;

            await repo.UpdateQuoteAsync(q);

            ReviewQuote rQ = await FillReviewQuote(q);

            return View("ReviewQuote", rQ);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewQuote(int quoteID, string name, string orderNumber, decimal discount, string setup)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);
            await repo.UpdateQuoteSimpleAsync(q, "Name", name);
            await repo.UpdateQuoteSimpleAsync(q, "OrderNum", orderNumber);
            await repo.UpdateQuoteSimpleAsync(q, "Discount", discount.ToString());
            await repo.UpdateQuoteSimpleAsync(q, "AddSetup", setup);
            var rQ = await FillReviewQuote(q);
            return View("ReviewQuote", rQ);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteShape(int quoteID, int shapeID, string returnUrl)
        {
            DeleteShape dS = new DeleteShape
            {
                Quote = await repo.GetQuoteByIdAsync(quoteID),
                Shape = await repoS.GetShapeByIdAsync(shapeID),
                ReturnUrl = returnUrl
            };
            return View(dS);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShape(DeleteShape dS)
        {
            dS.Quote = await repo.GetQuoteByIdAsync(dS.QuoteID);
            dS.Shape = await repoS.GetShapeByIdAsync(dS.ShapeID);
            await repoS.DeleteShapeAsync(dS.Shape);
            return RedirectToAction(dS.ReturnUrl, new { quoteID = dS.Quote.QuoteID });
        }

        public async Task<IActionResult> ReviewOpen(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);

            List<Formula> useFormulas = await CanUseFormulas(q);

            for (int i = 0; i < q.Shapes.Count; i++)
            {
                Shape s = q.Shapes[i];
                s.Legs.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
                string shapeNum = i < KnownObjects.NumberPrefix.Count ? KnownObjects.NumberPrefix[i] : (i + 1).ToString();
                decimal cutLength = 0;
                
                if (useFormulas.Count == 0)
                {
                    cutLength = await CalculateShapeLengthAsync(s); //Here's where Jeff jumps in
                }
                else
                {
                    cutLength = Calculations.Total_Shape_Length(s);
                    //Do jeff stuff
                    //pass in bar size as bar type, pass in legs as crude legs
                }
                if (cutLength > 240)
                {
                    ModelState.AddModelError(string.Empty, "The " + shapeNum + " shape cuts to longer than 240 inches so it was deleted.");
                    await repoS.DeleteShapeAsync(s);
                }
            }

            string errorMessage = GenerateInvalidFormulaErrorMessage(useFormulas);
            if (errorMessage != "")
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            ReviewQuote rQ = await FillReviewQuote(q);

            ReviewOpen rO = new ReviewOpen 
            { 
                ReviewQuote = rQ
            };

            return View(rO);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewOpen(ReviewOpen rO, string pickedUp)
        {
            Quote q = await repo.GetQuoteByIdAsync(rO.QuoteID);
            await repo.UpdateQuoteSimpleAsync(q, "Name", rO.Name);
            await repo.UpdateQuoteSimpleAsync(q, "OrderNum", rO.OrderNumber);
            await repo.UpdateQuoteSimpleAsync(q, "Discount", rO.Discount.ToString());
            await repo.UpdateQuoteSimpleAsync(q, "AddSetup", rO.Setup);
            if (pickedUp == null)
                pickedUp = "false";
            await repo.UpdateQuoteSimpleAsync(q, "PickedUp", pickedUp);
            var rQ = await FillReviewQuote(q);
            rO.ReviewQuote = rQ;

            for (int i = 0; i < rQ.Shapes.Count; i++)
            {
                Shape s = await repoS.GetShapeByIdAsync(rQ.Shapes[i].ShapeID);
                s.NumCompleted = rO.Completed[i];
                Shape newS = new Shape
                {
                    BarSize = s.BarSize,
                    Qty = s.Qty,
                    NumCompleted = rO.Completed[i]
                };
                foreach (Leg l in s.Legs)
                    newS.Legs.Add(l);
                await repoS.UpdateShapesAsync(s, newS);
                rQ.Shapes[i].Completed = rO.Completed[i];
            }

            return View(rO);
        }

        public async Task<IActionResult> UpdateQuotePrices(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);

            q = await UpdatePrices(q);

            await repo.UpdateQuoteAsync(q);

            ReviewQuote rQ = await FillReviewQuote(q);

            ReviewOpen rO = new ReviewOpen
            {
                ReviewQuote = rQ
            };

            return View("ReviewOpen", rO);
        }

        public async Task<IActionResult> CloseOpenQuote(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);
            await repo.UpdateQuoteSimpleAsync(q, "Open", "false");
            return RedirectToAction("ReviewClosed", new { quoteID = quoteID });
        }

        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> DeleteQuote(int quoteID, string returnUrl)
        {
            DeleteQuote dQ = new DeleteQuote
            {
                Quote = await repo.GetQuoteByIdAsync(quoteID),
                ReturnUrl = returnUrl
            };
            return View(dQ);
        }

        [HttpPost]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> DeleteQuote(DeleteQuote dQ)
        {
            dQ.Quote = await repo.GetQuoteByIdAsync(dQ.QuoteID);
            await repo.DeleteQuoteAsync(dQ.Quote);
            return RedirectToAction(dQ.ReturnUrl);
        }

        public async Task<IActionResult> ReviewClosed(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);

            ReviewQuote rQ = await FillReviewQuote(q);

            return View(rQ);
        }

        public async Task<IActionResult> OpenClosedQuote(int quoteID)
        {
            Quote q = await repo.GetQuoteByIdAsync(quoteID);
            await repo.UpdateQuoteSimpleAsync(q, "Open", "true");
            return RedirectToAction("ReviewOpen", new { quoteID = quoteID });
        }

        #region Methods for ReviewQuote

        private async Task<ReviewQuote> FillReviewQuote(Quote q)
        {
            ReviewQuote rQ = new ReviewQuote();
            rQ.QuoteID = q.QuoteID; //done
            rQ.Author = q.Author;
            rQ.DateQuoted = q.DateQuoted;
            rQ.Name = q.Name; //done
            rQ.OrderNum = q.OrderNum; //done
            rQ.AddSetup = q.AddSetup;
            rQ.Discount = q.Discount;

            rQ.NeededFormulas = await CanUseFormulas(q);
            if (rQ.NeededFormulas.Count == 0 && !q.UseFormulas)
                await repo.UpdateQuoteSimpleAsync(q, "UseFormulas", "true");
            else if (rQ.NeededFormulas.Count > 0 && q.UseFormulas)
                await repo.UpdateQuoteSimpleAsync(q, "UseFormulas", "false");
            rQ.UseFormulas = q.UseFormulas;

            rQ.PickedUp = q.PickedUp;

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

        //Creates a ReviewShape for every shape in the quote. This method adds the simple
        //data before calling FillShapes which calculates remnants.
        private async Task<List<ReviewShape>> CreateShapesAsync(Quote q)
        {
            List<ReviewShape> rSList = new List<ReviewShape>();
            foreach (Shape s in q.Shapes)
            {
                s.Legs.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
                ReviewShape rS = new ReviewShape();
                rS.QuoteID = q.QuoteID;
                rS.ShapeID = s.ShapeID;
                rS.Qty = s.Qty;
                rS.BarSize = s.BarSize;
                if (q.UseFormulas)
                {
                    rS.CutLength = await CalculateShapeLengthAsync(s); //Here's where Jeff jumps in
                }
                else
                {
                    rS.CutLength = Calculations.Total_Shape_Length(s);
                    //Do jeff stuff
                    //pass in bar size as bar type, pass in legs as crude legs
                }
                rS.Completed = s.NumCompleted;
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
                                        PerType = KnownObjects.Remnant,
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
                                        PerType = KnownObjects.Remnant,
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
                                        PerType = KnownObjects.Remnant,
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
                                        PerType = KnownObjects.Remnant,
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
            int perBar = GetShapesPerBar(rS.CutLength, KnownObjects.FullBarLength, out remnant);
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
                    PerLength = KnownObjects.FullBarLength,
                    PerType = KnownObjects.FullBar,
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
            if (GetShapesPerFinalBar(rS.Qty, rS.CutLength, KnownObjects.FullBarLength, out finalRemnant, out finalPerBar))
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
                    PerLength = KnownObjects.FullBarLength,
                    PerType = KnownObjects.FullBar,
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
                rL.LegID = l.LegID;
                rL.Length = l.Length;
                rL.Degree = l.Degree;
                rL.IsRight = l.IsRight;
                if (l.Mandrel != null)
                    rL.Mandrel = l.Mandrel.Name;
                else
                    rL.Mandrel = "";
                Formula f = await GetFormulaByLegAsync(l, s.BarSize);
                if (f != null)
                {
                    rL.PinNumber = f.PinNumber;
                    rL.InGained = f.InGained;
                }
                else
                {
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

            foreach (Cost c in q.Costs)
            {
                if (c.Name == KnownObjects.SetupCost)
                    setup = c.Price;
            }

            if (q.AddSetup == false)
                return 0;
            else if (q.AddSetup == true)
                return setup;
            else
            {
                foreach (Shape s in q.Shapes)
                {
                    if (s.Qty >= 100)
                        return 0;
                }
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
                if (!(rS.Legs.Count == 1 && rS.Legs[0].Length == KnownObjects.FullBarLength))
                    numOfCuts += rS.Qty;
                numOfBends += rS.Qty * (rS.Legs.Count - 1);
            }

            result.NumOfBars = numOfBars;
            result.NumOfCuts = numOfCuts;
            result.NumOfBends = numOfBends;

            foreach (Cost c in q.Costs)
            {
                if (c.Name == result.BarSize + KnownObjects.BarCost)
                    result.BarCost = c.Price;
                else if (c.Name == result.BarSize + KnownObjects.CutCost)
                    result.CutCost = c.Price;
                else if (c.Name == result.BarSize + KnownObjects.BendCost)
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
            var costsQuote = await repoC.BarCosts;//get costs from first quote? seeded?
            if (ModelState.IsValid)
            {
                for (int i = 0; i < 15; i++)//could be foreach too 
                {
                    if (costsQuote.FirstOrDefault(c => c.CostID == i).Price != Costs[i].Price)
                    {
                        await repoC.UpdateCostAsync(costsQuote.FirstOrDefault(c => c.CostID == i), Costs[i]);
                    }
                }
                costsQuote = await repoC.BarCosts;
                return View(costsQuote.ToList());
            }
            return View(costsQuote.ToList());
        }

        private async Task<List<Formula>> CanUseFormulas(Quote q)
        {
            var result = new List<Formula>();
            foreach (Shape s in q.Shapes)
            {
                foreach (Leg l in s.Legs)
                {
                    if (l != s.Legs.Last())
                    {
                        if (await GetFormulaByLegAsync(l, s.BarSize) == null)
                        {
                            var formula = new Formula
                            {
                                BarSize = s.BarSize,
                                Degree = l.Degree,
                                Mandrel = l.Mandrel
                            };
                            bool add = true;
                            foreach (Formula f in result)
                                if (formula.BarSize == f.BarSize && formula.Degree == f.Degree && formula.Mandrel == f.Mandrel)
                                    add = false;
                            if (add)
                                result.Add(formula);
                        }
                    }
                }
            }
            return result;
        }

        private bool VaildFormula(Formula f)
        {
            string m = f.Mandrel.Name;
            switch (f.BarSize)
            {
                case 3:
                    return true;
                case 4:
                    {
                        if (m == KnownObjects.NoneMandrel.Name)
                            return false;
                        if (m == KnownObjects.SmallMandrel.Name)
                            return true;
                        if (m == KnownObjects.MediumMandrel.Name)
                            return true;
                        if (m == KnownObjects.LargeMandrel.Name)
                            return true;
                        else
                            return false;
                    }
                case 5:
                    {
                        if (m == KnownObjects.NoneMandrel.Name)
                            return false;
                        if (m == KnownObjects.SmallMandrel.Name)
                            return false;
                        if (m == KnownObjects.MediumMandrel.Name)
                            return true;
                        if (m == KnownObjects.LargeMandrel.Name)
                            return true;
                        else
                            return false;
                    }
                case 6:
                    {
                        if (m == KnownObjects.NoneMandrel.Name)
                            return false;
                        if (m == KnownObjects.SmallMandrel.Name)
                            return false;
                        if (m == KnownObjects.MediumMandrel.Name)
                            return false;
                        if (m == KnownObjects.LargeMandrel.Name)
                            return true;
                        else
                            return false;
                    }
                default:
                    return false;
            }
        }

        private string GenerateInvalidFormulaErrorMessage(List<Formula> fList)
        {
            List<Formula> errors = new List<Formula>();
            foreach (Formula f in fList)
                if (!VaildFormula(f))
                    errors.Add(f);

            if (errors.Count > 0)
            {
                string message = "WARNING: The following bends will be impossible to create formulas for due to the mandrels being to large for the bar size. ";
                foreach (Formula f in errors)
                    message += "(Bar Size: " + f.BarSize + ", Degree: " + f.Degree + ", Mandrel: " + f.Mandrel.Name + ") ";
                return message;
            }
            else
                return "";
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        private async Task<Quote> UpdatePrices(Quote quote)
        {
            for (int i = quote.Costs.Count - 1; i >= 0; i--)
            {
                await repoC.DeleteCostByIdAsync(quote.Costs[i].CostID);
            }
            quote.Costs.Clear();

            var costsQuote = await repoC.BarCosts;//get costs from first quote? seeded?
            //var sumLegs = 0m;
            //var total = 0m;

            foreach (int barSize in KnownObjects.ValidRebarSizes)
            {
                Cost bar = null;
                Cost cut = null;
                Cost bend = null;
                switch(barSize)
                {
                    case 3:
                        bar = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar3GlobalCost.Name);
                        cut = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar3CutCost.Name);
                        bend = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar3BendCost.Name);
                        break;
                    case 4:
                        bar = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar4GlobalCost.Name);
                        cut = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar4CutCost.Name);
                        bend = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar4BendCost.Name);
                        break;
                    case 5:
                        bar = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar5GlobalCost.Name);
                        cut = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar5CutCost.Name);
                        bend = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar5BendCost.Name);
                        break;
                    case 6:
                        bar = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar6GlobalCost.Name);
                        cut = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar6CutCost.Name);
                        bend = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.Bar6BendCost.Name);
                        break;
                }
                var barCost = new Cost
                {
                    Name = barSize.ToString() + KnownObjects.BarCost,
                    Price = costsQuote.FirstOrDefault(c => c.Name == bar.Name).Price,
                    LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
                };
                if (!quote.Costs.Contains(barCost))//if it doesnt contain add cost
                    quote.Costs.Add(barCost);//Adding Cost per Bar

                var cutCost = new Cost
                {
                    Name = barSize.ToString() + KnownObjects.CutCost,
                    Price = costsQuote.FirstOrDefault(c => c.Name == cut.Name).Price,
                    LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
                };
                if (!CostExists(quote.Costs, cutCost.Name))
                    quote.Costs.Add(cutCost);//Adding Cost per cut
                //total += costsQuote.Costs.Find(c => c.Name == shape.BarSize.ToString() + "Cut").Price*quote.; //add cut prices later?

                var bendCost = new Cost
                {
                    Name = barSize.ToString() + KnownObjects.BendCost,
                    Price = costsQuote.FirstOrDefault(c => c.Name == bend.Name).Price,
                    LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
                };
                if (!CostExists(quote.Costs, bendCost.Name))
                    quote.Costs.Add(bendCost);//Adding Cost per bend
                //total += bendCost.Price;
            }

            var setupCost = new Cost
            {
                Name = KnownObjects.SetupCost,
                Price = costsQuote.FirstOrDefault(c => c.Name == KnownObjects.SetupCharge.Name).Price,
                LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
            };
            quote.Costs.Add(setupCost);

            return quote;
        }

        private bool CostExists(List<Cost> costs, string name)
        {
            foreach (Cost c in costs)
                if (c.Name == name)
                    return true;
            return false;
        }

        public IActionResult Canvas()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditShape(ReviewShape shape)
        {
            var oldShape = await repoS.GetShapeByIdAsync(shape.ShapeID);
            var newShape = new Shape()
            {
                ShapeID = shape.ShapeID,
                BarSize = shape.BarSize,
                Qty = shape.Qty,
                NumCompleted = shape.Completed,
                LegCount = shape.Legs.Count()
            };
            

            List<Leg> newLegs = new List<Leg>();
            for (int legIndex = 0; legIndex<shape.Legs.Count; legIndex++)
            {
                var newLeg = new Leg()
                {
                    Degree = shape.Legs[legIndex].Degree,
                    Mandrel = await repoF.GetMandrelByNameAsync(shape.Legs[legIndex].Mandrel),
                    IsRight = shape.Legs[legIndex].IsRight,
                    Length = shape.Legs[legIndex].Length,
                    SortOrder = shape.Legs[legIndex].SortOrder
                };
                newShape.Legs.Add(newLeg);

            }
            if (shape.QuoteID >0 && shape.ShapeID >0)
            {
                await repoS.UpdateShapesAsync(oldShape, newShape);
            }
            
            if (shape.ReviewOpen == true)
            {
                return RedirectToAction("ReviewOpen", new { quoteID = shape.QuoteID });
            }
            else
            {
                return RedirectToAction("ReviewQuote", new { quoteID = shape.QuoteID });
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewShape(ReviewShape shape)
        {
            var newShape = new Shape()
            {
                ShapeID = shape.ShapeID,
                BarSize = shape.BarSize,
                Qty = shape.Qty,
                NumCompleted = shape.Completed,
                LegCount = shape.Legs.Count()
            };


            List<Leg> newLegs = new List<Leg>();
            for (int legIndex = 0; legIndex < shape.Legs.Count; legIndex++)
            {
                var newLeg = new Leg()
                {
                    Degree = shape.Legs[legIndex].Degree,
                    Mandrel = await repoF.GetMandrelByNameAsync(shape.Legs[legIndex].Mandrel),
                    IsRight = shape.Legs[legIndex].IsRight,
                    Length = shape.Legs[legIndex].Length,
                    SortOrder = shape.Legs[legIndex].SortOrder
                };
                newShape.Legs.Add(newLeg);

            }

            if (shape.QuoteID > 0)
                    await repoS.AddShapeAsync(shape.QuoteID, newShape);
            if (shape.ReviewOpen == true)
            {
                return RedirectToAction("ReviewOpen", new { quoteID = shape.QuoteID });
            }
            else
            {
                return RedirectToAction("ReviewQuote", new { quoteID = shape.QuoteID });
            }
        }
        [HttpPost]
        public async Task<JsonResult> CheckIfValidShape(Quote q)
        {
            foreach (Leg l in q.Shapes[0].Legs)
            {
                if (l.Mandrel.Name != null)
                {
                    l.Mandrel = await repoF.GetMandrelByNameAsync(l.Mandrel.Name);
                }
            }
            q.Shapes[0].Legs.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
            decimal cutLength = 0;
            List<Formula> useFormulas = await CanUseFormulas(q);
            if (useFormulas.Count == 0)
            {
                cutLength = await CalculateShapeLengthAsync(q.Shapes[0]);
            }
            else
            {
                cutLength = Calculations.Total_Shape_Length(q.Shapes[0]);
            }
            if (cutLength > 240)
                return Json(false);
            else
                return Json(true);
        }
        
    }
}
