namespace CinemaWorld.Models.ViewModels.Ratings
{
    using System;

    public class StarRatingResponseModel
    {
        public int StarRatingsSum { get; set; }

        public string ErrorMessage { get; set; }

        public string AuthenticateErrorMessage { get; set; }

        public DateTime NextVoteDate { get; set; }
    }
}
