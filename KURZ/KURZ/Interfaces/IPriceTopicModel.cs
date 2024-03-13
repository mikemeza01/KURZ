using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IPriceTopicModel
    {

        public List<Price_Topics_Teacher> Price_Topics_Teacher(int ID_TEACHER);

        public string CreateTopicTeacher(Price_Topics Price_Topic);

        public Price_Topics TopicTeacherDetail(int? ID);

        public string EditTopicTeacher(Price_Topics Price_Topic);

        public int DeleteTopicTeacher(Price_Topics Price_Topic);

    }
}
