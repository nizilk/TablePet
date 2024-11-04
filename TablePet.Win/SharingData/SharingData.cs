namespace TablePet.Win.SharingDatan
{
    public static class SharingData
    {
        public static int Favorability { get; set; } = 80;
        
        public static bool IsFavorabilityLow()
        {
            return SharingData.Favorability < 30;
        }
    }
    
}