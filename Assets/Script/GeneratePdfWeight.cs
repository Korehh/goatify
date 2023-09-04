using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Syncfusion.Drawing;

public class GeneratePdfWeight : MonoBehaviour
{
    [System.Serializable]
    public class GoatDetail
    {
        public string date;
        public string notes;
        public float weight;
    }

    [System.Serializable]
    public class GoatData
    {
        public string name;
        public List<GoatDetail> details;
    }

    [System.Serializable]
    public class GoatDataList
    {
        public List<GoatData> dataList;
    }

    public void GeneratePdfFromJsonFile()
    {
        // Read JSON data from the file
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "weightdata.json");
        // Read the JSON file
        string jsonData = File.ReadAllText(jsonFilePath);
        GoatDataList data = JsonUtility.FromJson<GoatDataList>(jsonData);

        // Create a new PDF document
        PdfDocument document = new PdfDocument();
        document.PageSettings.Orientation = PdfPageOrientation.Landscape;
       // Add a page to the document with landscape orientation
        PdfPage page = document.Pages.Add();

        // Create PDF graphics for the page
        PdfGraphics graphics = page.Graphics;

         // Add an image to the top of the page
        string imagePath = Path.Combine(Application.dataPath, "Images", "glogo.png");
        using (FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        {
        PdfBitmap image = new PdfBitmap(imageStream);

        PointF imagePosition = new PointF(250, 0); // Adjust the position (x, y)
        SizeF imageSize = new SizeF(page.GetClientSize().Width - 500, 200); // Adjust the width and height

        graphics.DrawImage(image, imagePosition, imageSize);
        }

          // Add text below the image
        string text = "Date: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
        PointF textPosition = new PointF(300, 160); // Adjust the position (x, y)

        graphics.DrawString(text, textFont, PdfBrushes.Black, textPosition);
        // Create a DataTable from JSON data
        DataTable dataTable = ConvertToDataTable(data);

        // Create a PDF grid
        PdfGrid pdfGrid = new PdfGrid();
        pdfGrid.DataSource = dataTable;
        
        
        // Draw the PDF grid below the image
        pdfGrid.Draw(page, new PointF(0, 200)); // Start below the image

        // Set the standard font
        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

        PdfFont waterfont = new PdfStandardFont(PdfFontFamily.Helvetica, 50);
        //Add watermark text.
        PdfGraphicsState state = graphics.Save();
        graphics.SetTransparency(0.25f);
        graphics.RotateTransform(-40);
        graphics.DrawString("Alprince Goat Property!!!!", waterfont, PdfPens.Red, PdfBrushes.Red, new PointF(-150, 450));

        // Create a memory stream to save the PDF for mobile
        using (MemoryStream stream = new MemoryStream())
        {

            document.Save(stream);
            File.WriteAllBytes("weight.pdf", stream.ToArray());

            /*  for mobile
            document.Save(stream);

            // Save the PDF file to the Download directory
            string outputPath = Path.Combine(Application.persistentDataPath, "Download", "output.pdf");

            // Ensure the Download directory exists
            string downloadDir = Path.Combine(Application.persistentDataPath, "Download");
            Directory.CreateDirectory(downloadDir);

            // Save the PDF file to the Download directory
            File.WriteAllBytes(outputPath, stream.ToArray());

            // Close the PDF document
            document.Close(true);
            */

            // Open the generated PDF using the default PDF viewer
            //OpenPDF(outputPath);
            // Open the generated PDF using the default PDF viewer
            System.Diagnostics.Process.Start("weight.pdf");
        }
    }

    private void OpenPDF(string filePath)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
        
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);

        intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "application/pdf");
        intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        currentActivity.Call("startActivity", intentObject);
    }


    private DataTable ConvertToDataTable(GoatDataList dataList)
    {
        DataTable dataTable = new DataTable();

        // Add columns to DataTable
        dataTable.Columns.Add("Name", typeof(string));
        dataTable.Columns.Add("Date", typeof(string));
        dataTable.Columns.Add("Notes", typeof(string));
        dataTable.Columns.Add("Weight", typeof(float));

        // Populate the DataTable with data from GoatDataList
        foreach (var goat in dataList.dataList)
        {
            foreach (var detail in goat.details)
            {
                dataTable.Rows.Add(goat.name, detail.date, detail.notes, detail.weight);
            }
        }

        return dataTable;
    }
}
