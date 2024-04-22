using AKWAD_CAMP.Core.Interfaces;
using AKWAD_CAMP.Core.Models;
using AKWAD_CAMP.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Collections;
using AKWAD_CAMP.Web.Helpers;

namespace AKWAD_CAMP.Web.Services.Repos
{
    public class DataExportation : IDataExportation
    {
        readonly IUnitOfWork _context;
        private MemoryStream stream;
        private ExcelPackage package2;
        private ExcelWorksheet workSheet;


        public DataExportation(IUnitOfWork ufWork)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.stream = new MemoryStream();
            this.package2 =  new ExcelPackage(stream);
            this.workSheet = package2.Workbook.Worksheets.Add("Sheet1");
            _context = ufWork;
        }

        public MemoryStream ExportSuperTemp(int id)
        {
            var temp = _context.Templates.Get(criteria: e => e.TemplateId == id && e.TempOfTempInd, includes: new[] { "Sub", "TemOfTempsSup" }).FirstOrDefault();
            if (temp == null)
                return null;


            var colors = new Dictionary<int, color>();
            colors.Add(1, new color
            {
                seedpointR = 0,
                seedpointB = 32,
                seedpointG = 96
            });
            colors.Add(2, new color
            {
                seedpointR = 255,
                seedpointB = 230,
                seedpointG = 153
            });
            colors.Add(3, new color
            {
                seedpointR = 176,
                seedpointB = 52,
                seedpointG = 94
            });
            colors.Add(4, new color
            {
                seedpointR = 215,
                seedpointB = 132,
                seedpointG = 96
            });
            var stream = new MemoryStream();
            //required using OfficeOpenXml;
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                Dictionary<int, int> added = new();
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A1:F1"].Merge = true;
                workSheet.Cells[$"A1:F1"].Value = "التاريخ : " + DateTime.Now.ToString("dd/MM/yyyy");
                workSheet.Cells[$"A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["A2:F3"].Merge = true;
                workSheet.Cells[$"A2:F3"].Value = temp.TemplateName;
                workSheet.Cells[$"A2:F3"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                workSheet.Cells[$"A2:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A2:F3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"A2:F3"].Style.Fill.SetBackground(color: System.Drawing.Color.FromArgb(colors[1].seedpointR, colors[1].seedpointB, colors[1].seedpointG));
                double price = 0;
                int i = workSheet.Dimension.End.Row + 1;
                workSheet.InsertRow(i, 1);
                workSheet.Cells[$"A{i}:B{i}"].Merge = true;
                workSheet.Cells[$"A{i}:B{i}"].Value = "سعر الكميه";
                workSheet.Cells[$"A{i}:B{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A{i}:B{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"C{i}:C{i}"].Value = "الكمية";
                workSheet.Cells[$"C{i}:C{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"C{i}:C{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"D{i}:D{i}"].Value = "سعر الوحدة";
                workSheet.Cells[$"D{i}:D{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"D{i}:D{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"E{i}:E{i}"].Value = "اسم البيان";
                workSheet.Cells[$"E{i}:E{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"E{i}:E{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"F{i}:F{i}"].Value = "م";
                workSheet.Cells[$"F{i}:F{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"F{i}:F{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                i++;
                int m = 1;
                foreach (var item in temp.Sub)
                {
                    workSheet.InsertRow(i, 1);
                    var p = Helper.GetPrice(item.TemplateId);
                    var sub = temp.TemOfTempsSup?.FirstOrDefault(x => x.SubId == item.TemplateId);
                    workSheet.Cells[$"A{i}:B{i}"].Merge = true;
                    workSheet.Cells[$"A{i}:B{i}"].Value = (sub?.Quantity * p).ToString();
                    workSheet.Cells[$"A{i}:B{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"A{i}:B{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells[$"C{i}:C{i}"].Value = sub?.Quantity;
                    workSheet.Cells[$"C{i}:C{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"C{i}:C{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells[$"D{i}:D{i}"].Value = p.ToString();
                    workSheet.Cells[$"D{i}:D{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"D{i}:D{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells[$"E{i}:E{i}"].Value = item.TemplateName;
                    workSheet.Cells[$"E{i}:E{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"E{i}:E{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells[$"F{i}:F{i}"].Value = m++;
                    workSheet.Cells[$"F{i}:F{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"F{i}:F{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    i++;
                    price += sub.Quantity * p;
                }

                workSheet.Cells[$"A{i}:F{i+1}"].Merge = true;
                workSheet.Cells[$"A{i}:F{i+1}"].Value = (price) - (price * (double)(temp.Damage / 100)) + (price * (double)(temp.Profit / 100));
                workSheet.Cells[$"A{i}:F{i+1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A{i}:F{i+1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[$"A{i}:F{i+1}"].Style.Fill.SetBackground(color: System.Drawing.Color.FromArgb(colors[3].seedpointR, colors[3].seedpointB, colors[3].seedpointG));
                workSheet.Cells[$"A{i}:F{i + 1}"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }


        public MemoryStream ExportProduct(int id)
        {
            var template = _context.Templates.Get(criteria: e => e.TemplateId == id, includes: new[] { "TemplateItems" }).FirstOrDefault();
            

            var itemsList = template?.TemplateItems?.Select(e => new ItemsViewModel
            {
                ItemName = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemName,
                ItemNumber = e.ItemNumber,
                ItemPrice = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemPrice,
                QuantityPrice = ((_context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemPrice ?? 0) * (e.Quantity ?? 0)),
                ItemQuantity = e.Quantity ?? 0,
                ItemCategory = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.CategoryId,
                ItemUnit = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted, includes: new[] { "Unit" })?.FirstOrDefault()?.Unit?.UnitName
            }).ToList().GroupBy(x => x.ItemCategory).Select(x => new
            {
                x.Key,
                items = x.Select(i => new
                {
                    ItemName = i.ItemName,
                    ItemNumber = i.ItemNumber,
                    ItemPrice = i.ItemPrice,
                    QuantityPrice = i.QuantityPrice,
                    ItemQuantity = i.ItemQuantity,

                    ItemUnit = i.ItemUnit

                }),
                seqcout = catseq(x.Key).Count()
            }).OrderBy(x=>x.seqcout);



            //var categoris=itemsList.Select(s=>s.ItemCategory).Distinct().ToList();

            // query data from database   

            var colors =new Dictionary<int, color>();
            colors.Add(1, new color
            {
                seedpointR = 0,
                seedpointB = 32,
                seedpointG = 96
            });
            colors.Add(2, new color
            {
                seedpointR = 255,
                seedpointB = 230,
                seedpointG = 153
            });
            colors.Add(3, new color
            {
                seedpointR = 176,
                seedpointB = 52,
                seedpointG = 94
            });
            colors.Add(4, new color
            {
                seedpointR = 215,
                seedpointB = 132,
                seedpointG = 96
            });
            var stream = new MemoryStream();
            //required using OfficeOpenXml;
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
         
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                Dictionary<int, int> added = new();
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A1:F1"].Merge = true;
                workSheet.Cells[$"A1:F1"].Value = "التاريخ : " + DateTime.Now.ToString("dd/MM/yyyy");
                workSheet.Cells[$"A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                if (itemsList.Count() == 0)
                {
                    workSheet.Cells["A2:F2"].Merge = true;
                    workSheet.Cells[$"A2:F2"].Value = "لا يوجد عناصر";
                    workSheet.Cells[$"A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"A2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    package.Save();
                
                stream.Position = 0;
                return stream;
                }




                int i = 2;
                int level = 1;
                int maxlevel = itemsList.Max(x => x.seqcout);
            
            var start = _context.ItemsCategories.GetAll().GroupBy(x => x.ParentId).Select(x => new
            {
                ParentId = x.Key ?? 0,
                Childs = x.Select(i => i.CategoryId)
            });

            while (level <= maxlevel)
                {
                    var data = itemsList.Where(x => x.seqcout == level).ToList();
                    foreach (var item in data)
                    {
                        var seq = catseq(item.Key);
                        seq.Reverse();
                        List<int> headers_to_add = new();
                        if (level != 1)
                        {
                            foreach (var s in seq)
                            {
                                if(!added.Keys.Contains(s))
                                    headers_to_add.Add(s);
                            }
                            var index = seq.IndexOf(headers_to_add.First()) - 1 == -1 ? 0 : seq.IndexOf(headers_to_add.First()) - 1;

                            if(added.Keys.Contains(seq.ElementAt(index)))
                                i = added[seq.ElementAt(index)];
                            



                        }
                        else
                        {
                            headers_to_add.Add(item.Key.Value);
                        }


                        var count = headers_to_add.Count();
                        //workSheet.InsertRow(i, item.items.Count() + 5);
                        foreach (var cat in headers_to_add.Distinct())
                        {
                            if (!added.Keys.Contains((int)cat))
                            {
                                workSheet.InsertRow(i, 2);
                                var catname = _context.ItemsCategories.Find(cat).CategoryName;
                                workSheet.Cells[$"A{i}:F{i + 1}"].Merge = true;
                                workSheet.Cells[$"A{i}:F{i + 1}"].Value = template.TemplateName + $"({catname})";
                                workSheet.Cells[$"A{i}:F{i + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Cells[$"A{i}:F{i + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                if(level - count + 1 == 2)
                                {
                                    workSheet.Cells[$"A{i}:F{i + 1}"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                }
                                else
                                {
                                    workSheet.Cells[$"A{i}:F{i + 1}"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                                }
                                workSheet.Cells[$"A{i}:F{i + 1}"].Style.Fill.SetBackground(color: System.Drawing.Color.FromArgb(colors[level - count  +1].seedpointR, colors[level - count +1].seedpointB, colors[level - count + 1].seedpointG));
                                i += 2;
                            }
                            if (cat != headers_to_add.Last())
                                added.Add(cat, i);
                        }

                        workSheet.InsertRow(i, 1);
                        workSheet.Cells[i, 1].Value =  "سعر الكميه";
                        workSheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[i, 2].Value = "الوحدة";
                        workSheet.Cells[i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[i, 3].Value = "الكمية";
                        workSheet.Cells[i, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[i, 4].Value = "سعر الوحدة";
                        workSheet.Cells[i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[i, 5].Value = "اسم البيان";
                        workSheet.Cells[i, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[i, 6].Value = "م";
                        workSheet.Cells[i, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;





                        i++;
                        int m = 1;
                        foreach (var l in item.items)
                        {
                            workSheet.InsertRow(i, 1);
                            int j = 1;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = l.QuantityPrice;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = l.ItemUnit;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = l.ItemQuantity;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = l.ItemPrice;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = l.ItemName;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[i, j++].Value = m;
                            workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                            m++;
                            i++;
                        }
                        workSheet.InsertRow(i, 1);
                        workSheet.Cells[$"B{i}:F{i }"].Merge = true;
                        workSheet.Cells[$"B{i}:F{i}"].Value = $"الاجمالي:";
                        workSheet.Cells[$"B{i}:F{i }"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[$"B{i}:F{i }"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        workSheet.Cells[$"B{i}:F{i }"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        workSheet.Cells[$"B{i}:F{i }"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

                        workSheet.Cells[$"A{i}:A{i}"].Merge = true;
                        workSheet.Cells[$"A{i}:A{i}"].Value = item.items.Sum(x=>x.QuantityPrice);
                        workSheet.Cells[$"A{i}:A{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[$"A{i}:A{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        workSheet.Cells[$"A{i}:A{i}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        workSheet.Cells[$"A{i}:A{i}"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);
                        i += 1;
                        if (level != 1)
                        {
                            var parent = seq.ElementAt(level - 2);
                            var allsamelevelbutparent = added.Where(x => x.Value > added[parent]).Select(x => x.Key).ToList();
                            added.Where(x => allsamelevelbutparent.Contains(x.Key)).ToList().ForEach(x => added[x.Key] = added[x.Key] + item.items.Count() + 4);
                        }

                        added.Add(item.Key.Value , i);
                    }
                    level++;
                }
            
                var allseq = itemsList.Where(x=>x.Key !=null).Select(x => rever(catseq(x.Key))).Select(x => new {list= x, parent = x.First() }).GroupBy(x=>x.parent).Select(x=>x.SelectMany(x=>x.list).Distinct().ToList());
                foreach (var seq in allseq)
                {
                    var seqtotal = itemsList.Where (x=> x.Key != null && seq.Contains(x.Key.Value)).SelectMany(x => x.items).Sum(x => x.QuantityPrice);
                    var pos = added.Where(x => seq.Contains(x.Key)).Max(x => x.Value);
                    workSheet.InsertRow(pos, 1);
                    workSheet.Cells[$"B{pos}:F{pos }"].Merge = true;
                    workSheet.Cells[$"B{pos}:F{pos}"].Value = $"اجمالي الصنف : ";
                    workSheet.Cells[$"B{pos}:F{pos }"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"B{pos}:F{pos }"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    workSheet.Cells[$"B{pos}:F{pos }"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    workSheet.Cells[$"B{pos}:F{pos }"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

                    workSheet.Cells[$"A{pos}:A{pos}"].Merge = true;
                    workSheet.Cells[$"A{pos}:A{pos}"].Value = seqtotal;
                    workSheet.Cells[$"A{pos}:A{pos}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"A{pos}:A{pos}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    workSheet.Cells[$"A{pos}:A{pos}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    workSheet.Cells[$"A{pos}:A{pos}"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

                    added.Where(x => x.Value >= pos).ToList().ForEach(x => added[x.Key] = added[x.Key] + 1);

                }
                var total = itemsList.SelectMany(x => x.items).Sum(x => x.QuantityPrice);
                i = workSheet.Dimension.End.Row + 1;
                workSheet.InsertRow(i, 3);
                workSheet.Cells[$"C{i}:F{i + 2}"].Merge = true;
                workSheet.Cells[$"C{i}:F{i + 2}"].Value = $"الاجمالي الكلي";
                workSheet.Cells[$"C{i}:F{i + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"C{i}:F{i + 2}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells[$"C{i}:F{i + 2}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                workSheet.Cells[$"C{i}:F{i + 2}"].Style.Fill.SetBackground(color: System.Drawing.Color.Black);

                workSheet.Cells[$"A{i}:B{i+2}"].Merge = true;
                workSheet.Cells[$"A{i}:B{i+2}"].Value = total;
                workSheet.Cells[$"A{i}:B{i+2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A{i}:B{i+2}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells[$"A{i}:B{i+2}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                workSheet.Cells[$"A{i}:B{i+2}"].Style.Fill.SetBackground(color: System.Drawing.Color.Black);







                //foreach (var item in itemsList)
                //{


                //    if (workSheet.Dimension is not null)
                //        i = workSheet.Dimension.End.Row + 1;
                //    if (!added.Keys.Any(x => x == item.Key))
                //    {


                //        foreach (var cat in seq)
                //        {
                //            if (added.Keys.Contains(cat))
                //            {
                //                i = added[cat];
                //                continue;

                //            }

                //        }

                //        foreach (var cat in seq)
                //        {
                //            if (added.Keys.Contains(cat))
                //            {

                //                continue;
                //            }
                //            var catname = _context.ItemsCategories.Find(cat).CategoryName;
                //            workSheet.InsertRow(i, 2);
                //            workSheet.Cells[$"A{i}:F{i + 1}"].Merge = true;
                //            workSheet.Cells[$"A{i}:F{i + 1}"].Value = catname;
                //            workSheet.Cells[$"A{i}:F{i + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //            workSheet.Cells[$"A{i}:F{i + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //            workSheet.Cells[$"A{i}:F{i + 1}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                //            workSheet.Cells[$"A{i}:F{i + 1}"].Style.Fill.SetBackground(color: System.Drawing.Color.FromArgb(0,32 ,96 ));
                //            i += 2;

                //            added.Add(cat, i);
                //        }

                //    }
                //    else
                //    {
                //        i = added[item.Key.Value];
                //    }

                //    var alladdedtree = added.Keys.Select(x => rever(catseq(x))).Where(x => x.Contains(item.Key.Value)).Select(x=>Getsublist(item.Key.Value,x)).SelectMany(x => x).Distinct().ToList();


                //    added.Where(x => alladdedtree.Contains(x.Key)).ToList().ForEach(x => added[x.Key] = added[x.Key] + item.items.Count()+3);
                //    workSheet.InsertRow(i, 1);
                //    workSheet.Cells[i, 1].Value = "ItemName";
                //    workSheet.Cells[i, 2].Value = "ItemNumber";
                //    workSheet.Cells[i, 3].Value = "ItemPrice";
                //    workSheet.Cells[i, 4].Value = "ItemQuantity";
                //    workSheet.Cells[i, 5].Value = "QuantityPrice";
                //    workSheet.Cells[i, 6].Value = "ItemUnit";
                //    i++;
                //    foreach (var l in item.items)
                //    {
                //        int j = 1;
                //        workSheet.InsertRow(i, 1);
                //        workSheet.Cells[i, j++].Value = l.ItemName;
                //        workSheet.Cells[i, j++].Value = l.ItemNumber;
                //        workSheet.Cells[i, j++].Value = l.ItemPrice;
                //        workSheet.Cells[i, j++].Value = l.ItemQuantity;
                //        workSheet.Cells[i, j++].Value = l.QuantityPrice;
                //        workSheet.Cells[i, j++].Value = l.ItemUnit;
                //        i++;
                //    }

                //    workSheet.InsertRow(i, 2);
                //    workSheet.Cells[$"A{i}:F{i + 1}"].Merge = true;
                //    workSheet.Cells[$"A{i}:F{i + 1}"].Value = item.items.Sum(x=>x.QuantityPrice);
                //    i += 2;


                //}
                //var allseqofitems = itemsList.Select(x => new { x.Key, x.items, Parent = catseq(x.Key.Value).Last() , Last = catseq(x.Key.Value).First() });
                //var grouped = allseqofitems.GroupBy(x => x.Parent)
                //    .Select(x=>new { x.Key ,all_items =  x.SelectMany(x => x.items).ToList() ,Last = x.Select(x=>added[x.Last]).Max()});

                //foreach (var item in grouped)
                //{
                //    i = item.Last;
                //    workSheet.InsertRow(i, 2);
                //    workSheet.Cells[$"A{i}:F{i + 1}"].Merge = true;
                //    workSheet.Cells[$"A{i}:F{i + 1}"].Value = item.all_items.Sum(x=>x.QuantityPrice);
                //    added.Select(x => x).Where(x=>x.Value > i).ToList().ForEach(x => added[x.Key] = x.Value + 3);
                //}
                package.Save();
            }
            stream.Position = 0;
            return stream;

        }

        
        private List<int> Getsublist(int start,List<int> seq){


            return  seq.Where((value, index) => index >= seq.IndexOf(start) && index <= seq.Count() - 1)
                               .ToList();
        }

        private List<int> catseq(int? catid, bool? first = true)
        {

            if (catid == null)
                return new List<int>();
            List<int> seq = new List<int>();
            if (first.Value)
                seq.Add(catid.Value);
            var parent = _context.ItemsCategories.Find(catid.Value).ParentId;
            if (parent == null)
                return seq;
            seq.Add(parent.Value);
            return seq.Concat(catseq(parent, false)).ToList();
        }

        private List<int> rever (List<int> l)
        {
            l.Reverse();
            return l;
        }

        public class color
        {
            public int seedpointR { get; set; }
            public int seedpointB { get; set; }
            public int seedpointG { get; set; }
            
        }
        public MemoryStream ExportTemplate(int id)
        {
            throw new NotImplementedException();
        }


        public NTree<model> ConstructTree(int tempId)
        {
            var template = _context.Templates.Get(criteria: e => e.TemplateId == tempId, includes: new[] { "TemplateItems" }).FirstOrDefault();
            var catChilds = _context.ItemsCategories.Get(includes: new[] { "InverseParent" }).Select(x => new { cat = x.CategoryId, childs = x.InverseParent.Select(x => x.CategoryId) });
            var supParnts = _context.ItemsCategories.Get(criteria: x => x.ParentId == null).Select(x => x.CategoryId);
            
            var allcats = _context.ItemsCategories.GetAll().Select(x => x.CategoryId).OrderBy(x=>x);
            

            var itemsList = template?.TemplateItems?.Select(e => new ItemsViewModel
            {
                ItemName = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemName,
                ItemNumber = e.ItemNumber,
                ItemPrice = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemPrice,
                QuantityPrice = ((_context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.ItemPrice ?? 0) * (e.Quantity ?? 0)),
                ItemQuantity = e.Quantity ?? 0,
                ItemCategory = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted).FirstOrDefault()?.CategoryId,
                ItemUnit = _context.ItemsDetails.Get(criteria: i => i.ItemNumber == e.ItemNumber && i.Active & !i.Deleted, includes: new[] { "Unit" })?.FirstOrDefault()?.Unit?.UnitName
            }).Where(x=>x.ItemName is not null).ToList().GroupBy(x => x.ItemCategory).Select(x => new model
            {
                Key = x.Key??-1,
                itemsViewModels = x.Select(x=>x).ToList()
            });
            var oo = catChilds.Where(c => itemsList.Select(x => x.Key).Any(i => c.childs.Contains(i))).Select(x => x.cat);
            var supm = supParnts.Intersect(oo);
            NTree<model> tree = new NTree<model>(new model
            {
                Key = -1,
                itemsViewModels = itemsList?.Where(x=>x.Key==-1)?.SelectMany(x=>x.itemsViewModels).ToList()
            });
            foreach (var item in supm)
            {
                model model = itemsList?.FirstOrDefault(x => x.Key == item);
                
                if (model == null ) {
                        model = new model
                        {
                            Key = item
                        };
                }
                tree.AddChild(model);
            }
            foreach (var cat in allcats)
            {
                if (!supParnts.Contains(cat))
                {
                    var parent = catChilds.First(x => x.childs.Contains(cat)).cat;
                    model model = itemsList?.FirstOrDefault(x => x.Key == cat);
                    if (model == null)
                    {
                        continue;
                    }

                    TraverseAdd(ref tree, model, parent);
                }

            }
            return tree;
        }


        public MemoryStream Export(int id)
        {
           this.workSheet.Cells["A1:F1"].Merge = true;
           this.workSheet.Cells[$"A1:F1"].Value = "التاريخ : " + DateTime.Now.ToString("dd/MM/yyyy");
           this.workSheet.Cells[$"A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[$"A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            var tree = this.ConstructTree(id);
            tree.Traverse(PrintNode, PrintTotal);
            var price = Helper.GetPrice(id);
            int i = this.workSheet.Dimension.End.Row + 1;

            workSheet.InsertRow(i, 1);
            workSheet.Cells[$"B{i}:F{i }"].Merge = true;
            workSheet.Cells[$"B{i}:F{i}"].Value = $"الاجمالي الكلي :";
            workSheet.Cells[$"B{i}:F{i }"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[$"B{i}:F{i }"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[$"B{i}:F{i }"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            workSheet.Cells[$"B{i}:F{i }"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

            workSheet.Cells[$"A{i}:A{i}"].Merge = true;
            workSheet.Cells[$"A{i}:A{i}"].Value = price;
            workSheet.Cells[$"A{i}:A{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[$"A{i}:A{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[$"A{i}:A{i}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            workSheet.Cells[$"A{i}:A{i}"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);
            i += 1;
            this.package2.Save();
            this.package2.Dispose();
            this.stream.Position = 0;
            return this.stream;
        }

        private void PrintTotal (List<model> data , model parent)
        {
            if (parent.Key == -1)
                 return;


            var catName = _context.ItemsCategories.Find(parent.Key).CategoryName;

            int i = this.workSheet.Dimension.End.Row + 1;

            workSheet.InsertRow(i, 1);
            workSheet.Cells[$"B{i}:F{i }"].Merge = true;
            workSheet.Cells[$"B{i}:F{i}"].Value = $"اجمالي {catName}:";
            workSheet.Cells[$"B{i}:F{i }"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[$"B{i}:F{i }"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[$"B{i}:F{i }"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            workSheet.Cells[$"B{i}:F{i }"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

            workSheet.Cells[$"A{i}:A{i}"].Merge = true;
            workSheet.Cells[$"A{i}:A{i}"].Value = data.Sum(x => x.itemsViewModels.Sum(i=>i.QuantityPrice));
            workSheet.Cells[$"A{i}:A{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[$"A{i}:A{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[$"A{i}:A{i}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            workSheet.Cells[$"A{i}:A{i}"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);
            i += 1;
        }


        private void PrintNode(model data)
        {
            int i = this.workSheet.Dimension.End.Row + 1;
            if (data == null)
                return;
            if (data.Key == -1)
                return;

            var catName = _context.ItemsCategories.Find(data.Key).CategoryName;
            this.workSheet.InsertRow(i, 2);
            this.workSheet.Cells[$"A{i}:F{i + 1}"].Merge = true;
            this.workSheet.Cells[$"A{i}:F{i + 1}"].Value = catName;
            this. workSheet.Cells[$"A{i}:F{i+1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            this. workSheet.Cells[$"A{i}:F{i+1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            this. workSheet.Cells[$"A{i}:F{i+1}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            this.workSheet.Cells[$"A{ i}:F{ i + 1}"].Style.Fill.SetBackground(color: System.Drawing.Color.FromArgb((int)Color.seedpointR_1, (int)Color.seedpointB_1, (int)Color.seedpointG_1));



            if (data.itemsViewModels == null || !data.itemsViewModels.Any())
                return;



            i += 2;
            this.workSheet.InsertRow(i, 1);
           this.workSheet.Cells[i, 1].Value = "سعر الكميه";
           this.workSheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           this.workSheet.Cells[i, 2].Value = "الوحدة";
           this.workSheet.Cells[i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           this.workSheet.Cells[i, 3].Value = "الكمية";
           this.workSheet.Cells[i, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           this.workSheet.Cells[i, 4].Value = "سعر الوحدة";
           this.workSheet.Cells[i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           this.workSheet.Cells[i, 5].Value = "اسم البيان";
           this.workSheet.Cells[i, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           this.workSheet.Cells[i, 6].Value = "م";
           this.workSheet.Cells[i, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           this.workSheet.Cells[i, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            i++;
            int m = 1;
            foreach (var l in data.itemsViewModels)
            {
               this. workSheet.InsertRow(i, 1);
                int j = 1;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = l.QuantityPrice;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = l.ItemUnit;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = l.ItemQuantity;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = l.ItemPrice;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = l.ItemName;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             this. workSheet.Cells[i, j++].Value = m++;
             this. workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             this. workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                i++;
            }
            if (data.Key != -1)
            {
                workSheet.InsertRow(i, 1);
                workSheet.Cells[$"B{i}:F{i }"].Merge = true;
                workSheet.Cells[$"B{i}:F{i}"].Value = $"اجمالي {catName}:";
                workSheet.Cells[$"B{i}:F{i }"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"B{i}:F{i }"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells[$"B{i}:F{i }"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                workSheet.Cells[$"B{i}:F{i }"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);

                workSheet.Cells[$"A{i}:A{i}"].Merge = true;
                workSheet.Cells[$"A{i}:A{i}"].Value = data.itemsViewModels.Sum(x => x.QuantityPrice);
                workSheet.Cells[$"A{i}:A{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[$"A{i}:A{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells[$"A{i}:A{i}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                workSheet.Cells[$"A{i}:A{i}"].Style.Fill.SetBackground(color: System.Drawing.Color.LightSlateGray);
                i += 1;
            }
            

        }


        private void TraverseAdd(ref NTree<model> tree,model nodeData,int parent)
        {
          
            if (parent == tree.Data.Key)
            {
                tree.AddChild(nodeData);
                return;
            }
            foreach (var item in tree.Children)
            {
                
                var temp = item;
                TraverseAdd(ref temp, nodeData, parent);
                tree.ReplaceChild(temp,x=>x.Data.Key == temp.Data.Key);
            }


        }

        public class model
        {
            public model()
            {
                itemsViewModels = new List<ItemsViewModel>();
            }
            public int Key { get; set; }
            public List<ItemsViewModel> itemsViewModels { get; set; }
        }
       
        public enum Color
        {
            seedpointR_1 = 0,
            seedpointB_1 = 32,
            seedpointG_1 = 96,
            seedpointR_4 = 215,
            seedpointB_4 = 132,
            seedpointG_4 = 96,
            seedpointR_2 = 255,
            seedpointB_2 = 230,
            seedpointG_2 = 153,
            seedpointR_3 = 176,
            seedpointB_3 = 52,
            seedpointG_3 = 94
        }
        
       public class ItemsViewModel
        {
            [Display(Name = "ItemNumber")]
            public string ItemNumber { get; set; }
            [Display(Name = "ItemPrice")]
            public double? ItemPrice { get; set; }
            [Display(Name = "ItemName")]
            public string ItemName { get; set; }
            [Display(Name = "Quantity")]
            public double ItemQuantity { get; set; }
            [Display(Name = "QuantityPrice")]
            public double? QuantityPrice { get; set; }
            [Display(Name = "ItemCategory")]
            public int? ItemCategory { get; set; }
            [Display(Name = "ItemUnit")]
            public string ItemUnit { get; set; }

        }

    }
}
