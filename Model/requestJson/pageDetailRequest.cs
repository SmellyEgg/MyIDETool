namespace xinLongIDE.Model.requestJson
{
    public class pageDetailRequest
    {
        public int page_id = 0;

        public int time = 0;

        public string page_platform = string.Empty;

        public pageDetailRequest(int id, int time, string platform)
        {
            this.page_id = id;
            this.time = time;
            this.page_platform = platform;
        }


    }
}
