using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExifLib;

namespace PhotoRenamer {
    class Program {
        static void Main(string[] args) {
            var directory = args[0];
            var prefix = args[1];

            var di = new DirectoryInfo(directory);
            var files = di.GetFiles("*.jpg").Where(x => !x.Name.StartsWith(prefix));
            //var files = di.GetFiles("Job_2002*.jpg");
            List<RenameFileInfo> filesToRename = new List<RenameFileInfo>();
            foreach (var fi in files) {
                //try {
                //    using (ExifReader reader = new ExifReader(fi.FullName)) {
                //        // Extract the tag data using the ExifTags enumeration
                //        DateTime datePictureTaken;
                //        if (reader.GetTagValue<DateTime>(ExifTags.DateTimeOriginal,
                //            out datePictureTaken)) {
                //            filesToRename.Add(new RenameFileInfo() {
                //                FullName = fi.FullName,
                //                PhotoTaken = datePictureTaken
                //            });
                //        }
                //        else {
                //            filesToRename.Add(new RenameFileInfo() {
                //                FullName = fi.FullName,
                //                PhotoTaken = fi.CreationTime
                //            });
                //        }
                //    }
                //}
                //catch (Exception ex) {
                    filesToRename.Add(new RenameFileInfo() {
                        FullName = fi.FullName,
                        PhotoTaken = fi.CreationTime
                    });
                //}
            }

            foreach (var fileToRename in filesToRename) {
                int sequence = 0;
                
                string newFilename = Path.Combine(directory, string.Format("{0}_{1}_{2}.{3}", prefix, fileToRename.PhotoTaken.ToString("yyyyMMdd_HHmmss"), sequence, "jpg"));
                while (File.Exists(newFilename)) {
                    sequence++;
                    newFilename = Path.Combine(directory, string.Format("{0}_{1}_{2}.{3}", prefix, fileToRename.PhotoTaken.ToString("yyyyMMdd_HHmmss"), sequence, "jpg"));
                }
                
                Console.WriteLine("Renaming file {0} to {1}", fileToRename.FullName, newFilename);
                
                File.Move(fileToRename.FullName, newFilename);
            }

            Console.ReadLine();
        }
    }

    internal class RenameFileInfo {
        public string FullName { get; set; }
        public DateTime PhotoTaken { get; set; }
    }

}
