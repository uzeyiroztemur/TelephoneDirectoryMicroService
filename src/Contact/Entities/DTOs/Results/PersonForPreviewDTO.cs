namespace Entities.DTOs.Results
{
    public class PersonForPreviewDTO : PersonForViewDTO
    {
        public PersonForPreviewDTO()
        {
            ContactInfos = new List<PersonContactInfoForViewDTO>();
        }

        public List<PersonContactInfoForViewDTO> ContactInfos { get; set; }
    }
}