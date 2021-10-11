namespace ZarDevs.Http.Api.Tests.Serializers
{
    public class SerializerTestClass
    {
        public string Key1 {  get; set; }
        public int Key2 {  get; set; }

        public override string ToString()
        {
            return $"{nameof(Key1)}={Key1},{nameof(Key2)}={Key2}";
        }
    }
}
