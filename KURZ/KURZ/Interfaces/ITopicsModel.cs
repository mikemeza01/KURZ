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

    }
}
