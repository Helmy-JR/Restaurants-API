using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class CreateRestaurantDto
    {
        //[StringLength(100,MinimumLength =3)]
        public string Name { get; set; }
        public string Description { get; set; }

        //[Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        public bool HasDelivery { get; set; }

        //[EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string? ContactEmail { get; set; }

        //[Phone(ErrorMessage = "Please provide a valid contact number")]
        public string? ContactNumber { get; set; }


        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}