﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain;
using Backend.Models;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Authorize]
    public class LeaguesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Leagues
        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        // GET: Leagues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // GET: Leagues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
        {

            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";
                if (view.LogoFile!=null) {

                    pic = FilesHelper.UploadPhoto(view.LogoFile,folder);
                    pic = string.Format("{0}/{1}",folder,pic);
                }

                var league = ToLeague(view);
                league.Logo = pic;
                db.Leagues.Add(league);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        /***************CREAMOS TEAMS EQUIPOS EN LAS LIGAS*********************/
        public async Task<ActionResult> CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league= await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            var view = new TeamView { LeagueId=league.LeagueId,};
            return View(view);
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
           
            if (team == null)
            {
                return HttpNotFound();
            }
            var view = ToView(team);
            return View(view);
        }


        // POST: Teams/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Teams";
                if (view.LogoFile != null)
                {

                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;

                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", view.LeagueId));
            }
            return View(view);
        }

        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team= await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction(string.Format("Details/{0}", team.LeagueId));

        }





        // POST: Teams/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Teams";
                if (view.LogoFile != null)
                {

                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}",view.LeagueId));
            }
            return View(view);
        }


        private Team ToTeam(TeamView view)
        {
            return new Team
            {
                TeamId = view.TeamId,
                Name = view.Name,
                Logo = view.Logo,
                Initials = view.Initials,
                LeagueId = view.LeagueId,
                League=view.League,
            };
        }

        /*****************/
        private TeamView ToView(Team team)
        {
            return new TeamView
            {
                TeamId = team.TeamId,
                Name = team.Name,
                Logo = team.Logo,
                Initials = team.Initials,
                League=team.League,
                LeagueId=team.LeagueId,                
            };
        }

        // GET: Leagues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            var view = ToLeague(league); 
            return View(view);
        }

        private LeagueView ToLeague(League league)
        {
            return new LeagueView
            {
                
                LeagueId=league.LeagueId ,
                Name = league.Name,
                Logo = league.Logo,
                Teams = league.Teams
            };
        }

        // POST: Leagues/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic =view.Logo;
                var folder = "~/Content/Logos";
                if (view != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var league = ToLeague(view);
                league.Logo = pic;
                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Leagues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
            await db.SaveChangesAsync();
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
