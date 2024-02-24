using KURZ.Entities;
//using NuGet.DependencyResolver;

namespace KURZ.Helpers
{
    public class FilesHelper
    {
        public byte[] ReadFiles(string fileName, string uploadFolderPath) 
        {
			try
			{
                string photoPath = Path.Combine(uploadFolderPath, fileName);

                if (System.IO.File.Exists(photoPath))
                {
                    //Convierte la imagen de la ruta a bytes
                    byte[] imageBytes = System.IO.File.ReadAllBytes(photoPath);

                    //Guarda la imagen en bytes dentro de la variable ProfilePicture que se encuentra dentro de la clase UserDetails
                    return imageBytes;
                }

                return Array.Empty<byte>();
            }
			catch (Exception)
			{
                return Array.Empty<byte>();
			}
        }

        public void UploadFile(string fileName, string uploadFolderPath, IFormFile newProfilePicture)
        {
            try
            {

                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var filePath = Path.Combine(uploadFolderPath, fileName);

                // Guarda el archivo en la carpeta
                using var fileStream = new FileStream(filePath, FileMode.Create);
                newProfilePicture.CopyTo(fileStream);
            }
            catch (Exception)
            {
                
            }
        }

        public bool DeleteFile(string fileName, string uploadFolderPath)
        {
            try
            {
                var filePath = Path.Combine(uploadFolderPath, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    // Eliminar el archivo
                    System.IO.File.Delete(filePath);

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
