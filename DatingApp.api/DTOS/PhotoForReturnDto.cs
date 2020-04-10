using System;
namespace DatingApp.api.DTOS
{
    //Almost as the Same as PhotoForDetailed DTO(add new property:public ID)
    public class PhotoForReturnDto
    {
        
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool ismain { get; set; }

        public string PublicID { get; set; }
    }
}