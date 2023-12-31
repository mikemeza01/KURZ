﻿using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ITeacherModel
    {
        //Consultar a Michael
        //public List<Users>? TeacherList();
        public Users ValidateUser(Users teacher);

        public int UserCreate(Users teacher);

        public string base64Encode(string sData);

        public string base64Decode(string sData);
    }
}
