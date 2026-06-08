namespace E_CommerceSystem_API.DTOs
{
    public class PlaceOrderDTO
    {
        public int UserId { get; set; }

        public List<ItemDTO> Items { get; set; }

    }
}
