using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace TaskTimer.Models
{
    public class GoogleDocumentModel
    {
        //reference to Google API 
        public OAuth2Parameters GoogleAPIAccess { get; set; }

        //documents of Google API reference
        public AtomEntryCollection Documents { get; set; }

        //document current work with
        public SpreadsheetEntry Document { get; set; }

        //worksheets current work with
        public AtomEntryCollection Worksheets { get; set; }

        //worksheet current work with
        public WorksheetEntry Worksheet { get; set; }

        //worksheet data
        public AtomEntryCollection Table { get; set; }

        //worksheet for save
        public ListFeed TableSave { get; set; }

        //active row 
        public int? RowActive { get; set; }

        //active row in save table
        public ListEntry RowActiveSave { get; set; }
    }
}