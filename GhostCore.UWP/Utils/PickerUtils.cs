using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace GhostCore.UWP.Utils
{
    public static class PickerUtils
    {
        public static async Task<StorageFile> PickFileAsync(PickerViewMode viewMode = PickerViewMode.List, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, params string[] desiredExtensions)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = viewMode,
                SuggestedStartLocation = suggestedStartLocation,
            };

            if (desiredExtensions != null && desiredExtensions.Length > 0)
            {
                foreach (var extension in desiredExtensions)
                {
                    picker.FileTypeFilter.Add(extension);
                }
            }
            else
            {
                picker.FileTypeFilter.Add("*");
            }

            var file = await picker.PickSingleFileAsync();
            return file;
        }

        public static async Task<StorageFile> SaveFileAsync(string suggestedFileName)
        {
            var picker = new FileSavePicker
            {
                SuggestedFileName = suggestedFileName
            };
            picker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });

            var file = await picker.PickSaveFileAsync();
            return file;
        }

        public static async Task<StorageFolder> PickFolderAsync(PickerViewMode viewMode = PickerViewMode.List, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder)
        {
            var folderPicker = new FolderPicker()
            {
                ViewMode = viewMode,
                SuggestedStartLocation = suggestedStartLocation,
            };
            folderPicker.FileTypeFilter.Add("*");

            var folder = await folderPicker.PickSingleFolderAsync();
            return folder;
        }
    }
}

