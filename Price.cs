namespace RTSProject
{
    public struct Price
    {
        int ironCount;
        int electricityCount;

        public Price(int ironCount, int electricityCount)
        {
            this.ironCount = ironCount;
            this.electricityCount = electricityCount;
        }
    }
}