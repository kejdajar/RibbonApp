using RibbonApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RibbonApp.Database
{
    /// <summary>
    /// Třída starající se o ukládání dat do souborů -  BINÁRNÍ SERIALIZACE
    /// </summary>
    public class Serialization
    {
        
        /// <param name="directoryPath">Nastavuje adresář, kam se budou ukádat serializovaná data.</param>
        public Serialization(string directoryPath)
        {
            this._directoryPath = directoryPath;
        }

        private string _directoryPath;

        /// <summary>
        /// Vrací cestu k adresáři, kam se ukládají serializovaná data.
        /// </summary>
        /// <returns></returns>
        public string GetSaveDirectoryPath()
        {
            return this._directoryPath;
        }

        /// <summary>
        ///  Tato generická metoda nalezne soubor, který je vyhrazen pro uložení dat "T" a
        ///  vrátí tato data jako List.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DeserializationFailException"></exception>
        public List<T> Load<T> ()
        {
            string filename = typeof(T).Name + "Data.bin";
            string correspondingPath = System.IO.Path.Combine(_directoryPath, filename);

            try
            {
                using (FileStream fs = File.Open(correspondingPath, FileMode.Open))              
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    List<T> result = (List<T>)binaryFormatter.Deserialize(fs);
                    return result;
                }
            }
            catch
            {
                return default(List<T>);
            }
           
        }

        /// <summary>
        /// Tato generická metoda ukládá data typu "T" do souboru s názvem 
        /// "TData.dat".
        /// </summary>
        /// <param name="entities">Kolekce dat k uložení.</param>
        /// <exception cref="SerializationFailException"></exception>
        public void Save<T> (List<T> entities)
        {
            if (!entities.GetType().IsSerializable) throw new NotImplementedException();

            string filename = typeof(T).Name + "Data.bin";
            string correspondingPath = System.IO.Path.Combine(_directoryPath, filename);

            try
            {
                var loadedCollection = Load<T>();

                if (loadedCollection == null)
                {
                    using (FileStream fs = File.Open(correspondingPath, FileMode.Create))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(fs, entities);
                    }
                }
                else
                {
                    List<T> entitesSavedSoFar = new List<T>();
                    entitesSavedSoFar =  loadedCollection as List<T> ;
                    entitesSavedSoFar.AddRange(entities);
                    using (FileStream fs = File.Open(correspondingPath, FileMode.Open))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(fs, entitesSavedSoFar);
                    }

                }
            }
            catch(Exception ex)
            {
                throw new SerializationFailException(ex);
            }
                   

        }

       
        


    }
}
