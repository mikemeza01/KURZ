using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ITopicsModel
    {
        public string TopicsCreate(Topics topic);
        public List<TopicsView> TopicsList();

        public string TopicsEdit(Topics topic);

        public Topics TopicsDetail(int? ID);

        public TopicsView TopicsDetailView(int? ID);

        public int TopicDelete(Topics topic);

        public List<Topics> TopicsByCatSub(int ID_CATEGORY, int ID_SUBCATEGORY);
        public List<TeacherTopicsView> TopicByTeacherView(int ID);


        }
}
