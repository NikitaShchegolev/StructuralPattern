using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Text;

namespace Decorator.ExemplesClass
{
    public static class FileDecorator
    {
        private const string StringToWrite = "Мой дядя самых честных правил,\n" +
            "Когда не в шутку занемог,\n" +
            "Он уважать себя заставил\n" +
            "И лучше выдумать не мог.\n" +
            "Его пример другим наука;\n" +
            "Но, боже мой, какая скука\n" +
            "С больным сидеть и день и ночь,\n" +
            "Не отходя ни шагу прочь!\n" +
            "Какое низкое коварство\n" +
            "Полуживого забавлять,\n" +
            "Ему подушки поправлять,\n" +
            "Печально подносить лекарство,\n" +
            "Вздыхать и думать про себя:\n" +
            "Когда же черт возьмет тебя!";
        private const string Folder = "C://Users//n.shchegolev//source//repos//StructuralPattern2//Addapter//Decorator";
        

        public static void Write()
        {
            using (FileStream fileStream = new FileStream($"{Folder}/createFileTxt.txt", FileMode.Create))
            {
                byte[] buffer = Encoding.Default.GetBytes(StringToWrite);
                fileStream.Write(buffer);
            }
        }

        public static void WriteArchived()
        {
            using (FileStream fileStream = new FileStream($"{Folder}/archived.gz", FileMode.Create))
            using (GZipStream gZipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
            {
                byte[] bytes = Encoding.Default.GetBytes(StringToWrite);
                gZipStream.Write(bytes);
            }
        }
    }
}
