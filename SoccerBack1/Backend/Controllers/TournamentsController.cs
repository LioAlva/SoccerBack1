using Backend.Helpers;
using Backend.Models;
using Domain;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Tournaments
        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.ToListAsync());
        }

        //******************************************* TournamentTeams

        // GET: TournamentTeams/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentTeam tournamentTeam)
        {
            if (ModelState.IsValid)
            {
                db.TournamentTeams.Add(tournamentTeam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", tournamentTeam.TeamId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", tournamentTeam.TournamentGroupId);
            return View(tournamentTeam);
        }



        //*******************************************Dates
        // GET: Dates/Create
        public async Task<ActionResult> CreateDates(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var view = new Date
            {
                TournamentId=tournament.TournamentId
            };
            return View(view);
        }

        // POST: Dates/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDates(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Dates.Add(date);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
            }
            return View(date);
        }

        // GET: Dates/Edit/5
        public async Task<ActionResult> EditDates(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Date date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return HttpNotFound();
            }
            
            return View(date);
        }

        // POST: Dates/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDates(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Entry(date).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));

            }
            return View(date);
        }

        // GET: Dates/Edit/5
        public async Task<ActionResult> DeleteDates(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Date date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return HttpNotFound();
            }
            db.Dates.Remove(date);
            await db.SaveChangesAsync();
            return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
        }
        //END DATES**********************************************


        /************** TournamentTeams *****************/
        // GET: TournamentTeams/Edit/5
        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentTeam = await db.TournamentTeams.FindAsync(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }
            db.TournamentTeams.Remove(tournamentTeam);
            await db.SaveChangesAsync();
            return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));

        }


        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentTeam = await db.TournamentTeams.FindAsync(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name",tournamentTeam.Team.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(l => l.LeagueId ==tournamentTeam.Team.LeagueId).OrderBy(t => t.Name), "TeamId", "Name",tournamentTeam.Team.TeamId);
            var view = ToView(tournamentTeam);
            return View(view);
        }

        private TournamentTeamView ToView(TournamentTeam tournamentTeam)
        {
            return new TournamentTeamView {
                AgainstGoals=tournamentTeam.AgainstGoals,
                FavorGoals= tournamentTeam.FavorGoals,
                LeagueId = tournamentTeam.Team.LeagueId,
                MatchesLost = tournamentTeam.MatchesLost,
                MatchesPlayed= tournamentTeam.MatchesPlayed,
                MatchesWon = tournamentTeam.MatchesWon,
                Points = tournamentTeam.Points,
                Position = tournamentTeam.Position,
                Team = tournamentTeam.Team,
                TeamId = tournamentTeam.TeamId,
                TournamentGroup = tournamentTeam.TournamentGroup,
                TournamentGroupId = tournamentTeam.TournamentGroupId,
                TournamentTeamId = tournamentTeam.TournamentTeamId
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);
                db.Entry(tournamentTeam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));

            }
            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(l => l.LeagueId == view.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.TeamId);

            return View(view);
        }

        private TournamentTeam ToTournamentTeam(TournamentTeamView view)
        {
            return new TournamentTeam {
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
                Position = view.Position,
                Team = view.Team,
                TeamId = view.TeamId,
                TournamentGroup = view.TournamentGroup,
                TournamentGroupId = view.TournamentGroupId,
                TournamentTeamId = view.TournamentTeamId

            };
        }

        /********************** GROUP *****************************/
        // GET: Dates/Edit/5
        public async Task<ActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroups = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroups== null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroups);
            }



        // GET: Tournaments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            var view = ToTournamentView(tournament);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(view);
        }

        // GET: Tournaments/Create
        public async Task<ActionResult> CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroups = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroups == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t=>t.Name),"LeagueId","Name");
            ViewBag.TeamId = new SelectList(db.Teams.Where(l=>l.LeagueId==db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");

            var view = new TournamentTeamView{TournamentGroupId = tournamentGroups.TournamentGroupId};
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);
                db.TournamentTeams.Add(tournamentTeam);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("DetailsGroup/{0}",tournamentTeam.TournamentGroupId));
            }
            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(l => l.LeagueId == view.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.TeamId);

            return View(view);
        }

        /************************GROUPS******************/
        // GET: TournamentGroups/Create
        public async Task<ActionResult> CreateGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }
            var view = new TournamentGroup
            {
                TournamentId=tournament.TournamentId
            };
            return View(view);
        }

        // POST: TournamentGroups/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(tournamentGroup);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}",tournamentGroup.TournamentId));
            }

            return View(tournamentGroup);
        }


        //editgroup
        // GET: TournamentGroups/Edit/5
        public async Task<ActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            return View(tournamentGroup);
        }

        // POST: TournamentGroups/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
            }
            return View(tournamentGroup);
        }


   //aca no se que pasa un bag
        public async Task<ActionResult> DeleteGroup(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup==null)
            {
                return HttpNotFound();
            }
            db.TournamentGroups.Remove(tournamentGroup);
            await db.SaveChangesAsync();
            return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));

        }

        /***************************************************/


        private Tournament ToTournament(TournamentView view)
        {
            return new Tournament
            {
                TournamentId=view.TournamentId,
                Dates=view.Dates,
                IsActive=view.IsActive,
                Logo=view.Logo,
                Name=view.Name,
                Order=view.Order,
                Groups=view.Groups
            };
        }


        private TournamentView ToTournamentView(Tournament tournament)
        {
            return new TournamentView
            {
                TournamentId = tournament.TournamentId,
                Dates = tournament.Dates,
                IsActive = tournament.IsActive,
                Logo = tournament.Logo,
                Name = tournament.Name,
                Order = tournament.Order,
                Groups=tournament.Groups
            };
        }
        // GET: Tournaments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            var view = ToTournamentView(tournament);

            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(view);
        }

        // POST: Tournaments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";
                if (view.LogoFile != null)
                {

                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var tournament = ToTournament(view);
                tournament.Logo = pic;
                db.Entry(tournament).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            db.Tournaments.Remove(tournament);
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
