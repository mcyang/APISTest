﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APISTest.Models;

namespace APISTest.Controllers
{
    /// <summary>
    /// 基本資料管理-車廠資料
    /// </summary>
    public class CarMakersController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: CarMakers
        public ActionResult Index()
        {
            return View(db.CarMakers.ToList());
        }

        // GET: CarMakers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaker carMaker = db.CarMakers.Find(id);
            if (carMaker == null)
            {
                return HttpNotFound();
            }
            return View(carMaker);
        }

        // GET: CarMakers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarMakers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,IsDelete")] CarMaker carMaker)
        {
            if (ModelState.IsValid)
            {
                db.CarMakers.Add(carMaker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carMaker);
        }

        // GET: CarMakers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaker carMaker = db.CarMakers.Find(id);
            if (carMaker == null)
            {
                return HttpNotFound();
            }
            return View(carMaker);
        }

        // POST: CarMakers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,IsDelete")] CarMaker carMaker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carMaker).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carMaker);
        }

        // GET: CarMakers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaker carMaker = db.CarMakers.Find(id);
            if (carMaker == null)
            {
                return HttpNotFound();
            }
            return View(carMaker);
        }

        // POST: CarMakers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarMaker carMaker = db.CarMakers.Find(id);
            db.CarMakers.Remove(carMaker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}