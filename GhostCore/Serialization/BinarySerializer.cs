using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GhostCore.Serialization
{
    public static class BinarySerializer
    {
        public static void Serialize<T>(T data, string diskPath)
        {
            var fs = new FileStream(diskPath, FileMode.CreateNew);
            Serialize(data, fs, closeStream: true);
        }
        public static void Serialize<T>(T data, Stream stream, bool closeStream = false)
        {
            var formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(stream, data);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                if (closeStream)
                    stream.Close();
            }
        }

        public static T Deserialize<T>(string diskPath)
        {
            FileStream fs = new FileStream("DataFile.dat", FileMode.Open);
            var t = Deserialize<T>(fs, diskPath);
            fs.Close();

            return t;
        }
        public static T Deserialize<T>(Stream stream, string diskPath)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }
    }
}
