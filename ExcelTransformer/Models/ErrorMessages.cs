﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTransformer.Models
{
    public static class ErrorMessages
    {
        public const string PATH_NOT_FOUND = "File not found.";
        public const string FILE_PATH_REQUIRED = "File path required.";
        public const string TABLE_NAME_MISSING = "Table name is required.";
        public const string NO_PERMISSION = "You have not provided access to read file.";
        public const string ERROR_OCCURED = "An error occured.";
    }
}