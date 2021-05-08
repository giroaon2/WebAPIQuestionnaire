using System;

namespace WebAPIQuestionnaire
{
    public class Questionnaire
    {
        /// <summary>
        /// Id string empty will be add if id not null will be update
        /// </summary>
        public string Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Country { get; set; }
        public string AddressHouse { get; set; }
        public string AddressWork { get; set; }
        public Occupation Occupation { get; set; }
        public string JobTitle { get; set; }
        public string BusinessType { get; set; }

        public void UpdateQuestionnaire(Questionnaire update)
        {
            Title = update.Title;
            FirstName = update.FirstName;
            LastName = update.LastName;
            DateOfBirth = update.DateOfBirth;
            Country = update.Country;
            AddressHouse = update.AddressHouse;
            AddressWork = update.AddressWork;
            Occupation = update.Occupation;
            JobTitle = update.JobTitle;
            BusinessType = update.BusinessType;
        }
    }

    public class Occupation
    {
        public string OccupationId { get; set; }
        public string OccupationName { get; set; }
    }
}
