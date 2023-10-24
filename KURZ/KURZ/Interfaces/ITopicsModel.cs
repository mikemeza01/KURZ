using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ITopicsModel
    {
        public int TopicsCreate(Topics topic);
        public List<Topics> TopicsList();

    }
}
