using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Text;

namespace Addapter.ExemplesClass
{
    public class FileDecorator
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
        private const string folder = "C://Users//n.shchegolev//source//repos//OTUS_Patterns_Structural";
        

        public static void Write()
        {
            using (FileStream fileStream = new FileStream($"{folder}/createFileTxt.txt", FileMode.Create))
            {
                byte[] buffer = Encoding.Default.GetBytes(StringToWrite);
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void WriteArchived()
        {
            using (FileStream fileStream = new FileStream($"{folder}/generateZip.zip", FileMode.Create))
            using (GZipStream gZipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
            {
                byte[] bytes = Encoding.Default.GetBytes(StringToWrite);
                gZipStream.Write(bytes);
            }
        }
    }
}
