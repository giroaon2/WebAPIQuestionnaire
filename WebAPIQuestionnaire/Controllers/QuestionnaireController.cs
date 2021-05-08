using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIQuestionnaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        const string fileName = "questionnaire.csv";
        // ExportCsv: api/<QuestionnaireController>
        [HttpGet]
        public Task<FileResult> ExportCsv()
        {
            return Export();
        }

        // POST api/<QuestionnaireController>
        [HttpPost]
        public string Post([FromBody] Questionnaire message)
        {
            if (CountriesNotAllow(message))
            {
                return "the questionnaire is done";
            }
            if (message.Id == string.Empty || message.Id == "string")
            {
                message.Id = Guid.NewGuid().ToString();
            }
            WriteCsv(message, fileName);
            return "success";
        }

        private bool CountriesNotAllow(Questionnaire message)
        {
            return (message.Country == "Cambodia" || message.Country == "Myanmar" || message.Country == "Pakistan");
        }

        // DELETE api/<QuestionnaireController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            RemoveCsvById(id);
        }

        private async Task<FileResult> Export()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("Hello, World!");
            writer.Flush();
            stream.Position = 0;

            return File(ReadCsv(fileName), "text/csv", fileName);
        }
        private void RemoveCsvById(string id)
        {
            using StreamReader streamReader = System.IO.File.OpenText(fileName);
            var data = new List<Questionnaire>();
            ReadCsvToQuestionnaires(streamReader, data);

            foreach (var d in data)
            {
                if (d.Id == id)
                {
                    data.Remove(d);
                    break;
                }
            }

            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
            }
        }
        private void WriteCsv(Questionnaire questionnaire, string fileName)
        {
            using StreamReader streamReader = System.IO.File.OpenText(fileName);
            var data = new List<Questionnaire>();
            ReadCsvToQuestionnaires(streamReader, data);
            UpsertDataToCsv(questionnaire, data);

            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
            }
        }

        private void UpsertDataToCsv(Questionnaire questionnaire, List<Questionnaire> data)
        {
            bool isUpdate = false;
            foreach (var d in data)
            {
                if (d.Id == questionnaire.Id)
                {
                    isUpdate = true;
                    d.UpdateQuestionnaire(questionnaire);
                }
            }
            if (!isUpdate)
            {
                data.Add(questionnaire);
            }
        }

        private void ReadCsvToQuestionnaires(StreamReader streamReader, List<Questionnaire> data)
        {
            using (CsvReader csvReader = new CsvReader(streamReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.IgnoreBlankLines = false;

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<Questionnaire>();
                    data.Add(record);
                }
            }
        }

        private Stream ReadCsv(string fileName)
        {
            StreamReader streamReader = System.IO.File.OpenText(fileName);
            var csvReader = new CsvReader(streamReader);
            csvReader.Configuration.HasHeaderRecord = false;
            return streamReader.BaseStream;
        }
    }
}
