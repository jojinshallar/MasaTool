namespace ProjectArchitectureGenerator.Core.Cores.TemplateDatas
{
    internal class EntityTemplateData
    {
        public EntityTemplateData(string projectnamespace, string entityname, string idtype, string useridtype, bool includelocalusing)
        {
            this.projectnamespace = projectnamespace;
            this.entityname = entityname;
            this.idtype = idtype;
            this.useridtype = useridtype;
            this.includelocalusing = includelocalusing;
        }

        public string idtype { get; set; }
        public string useridtype { get; set; }
        public string projectnamespace { get; set; }
        public string entityname { get; set; }
        public bool includelocalusing { get; set; }
    }
}
