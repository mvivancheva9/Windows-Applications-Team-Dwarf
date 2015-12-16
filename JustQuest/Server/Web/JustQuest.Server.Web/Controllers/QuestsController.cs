﻿namespace JustQuest.Server.Web.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Models;

    [Authorize]
    public class QuestsController : ApiController
    {
        private IApplicationData data;

        public QuestsController(IApplicationData data)
        {
            this.data = data;
        }

        public QuestsController()
            : this(new ApplicationData())
        {
        }

        public IHttpActionResult Get()
        {
            var currentUserId = User.Identity.GetUserId();

            var result = this.data.Quests.All()
                .Where(x => x.NumberOfRemainingCorrectGuesses > 0 && x.UserId != currentUserId)
                .ProjectTo<QuestResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Get(int id)
        {
            var result = this.data.Quests.All()
                .FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return this.BadRequest("No Quest with that Id exists in the database.");
            }

            var model = Mapper.Map<QuestResponseModel>(result);

            return this.Ok(model);
        }

        public IHttpActionResult Put(int id, string answer)
        {
            var quest = this.data.Quests.All()
                .FirstOrDefault(x => x.Id == id);

            if (quest == null)
            {
                return this.BadRequest("No Quest with that Id exists in the database.");
            }

            var currentUserId = User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(x => x.Id == currentUserId);

            if (quest.UserId == currentUserId)
            {
                return this.BadRequest("You should not be here...");
            }

            // TODO: DECIDE WHETHER ANSWER VALIDATION WILL HAPPEN ON SERVER OR CLIENT

            bool isValidAnswer = quest.PossibleAnswers.Any(x => x == answer);

            if (isValidAnswer)
            {
                int pointsAwarded = 0;

                switch (quest.NumberOfRemainingCorrectGuesses)
                {
                    // if its the 3rd correct answer - award creator
                    case 1:
                        pointsAwarded = 20;
                        user.Rupees += pointsAwarded;
                        quest.NumberOfRemainingCorrectGuesses -= 1;

                        quest.User.Rupees += 100;
                        break;
                    case 2:
                        pointsAwarded = 50;
                        user.Rupees += pointsAwarded;
                        quest.NumberOfRemainingCorrectGuesses -= 1;
                        break;
                    case 3:
                        pointsAwarded = 100;
                        user.Rupees += pointsAwarded;
                        quest.NumberOfRemainingCorrectGuesses -= 1;
                        break;
                    default:
                        return this.BadRequest("This Quest has already been completed. Go look for other adventures!");
                }

                this.data.Quests.Update(quest);
                this.data.Users.Update(user);
                this.data.SaveChanges();

                return this.Ok(pointsAwarded);
            }
            else
            {
                return this.Ok(0);
            }
        }

        // TODO: replace with a request model
        public IHttpActionResult Post(Quest quest)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(x => x.Id == currentUserId);

            if (user.Rupees < 10)
            {
                return
                    this.BadRequest(
                        "Not enough rupees to create a quest. Go look for some adventures and come back later!");
            }

            user.Quests.Add(quest);

            this.data.Users.Update(user);
            this.data.SaveChanges();

            return this.Ok();
        }
    }
}