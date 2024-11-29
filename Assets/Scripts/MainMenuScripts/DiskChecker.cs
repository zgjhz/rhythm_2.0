using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class DiskChecker : MonoBehaviour
{
    [DllImport("SerialNumberDisk.dll")]
    public static extern String GetModelFromDrive(string drive);
    public string expectedDrive;
    private bool isLicensed;

    void Start()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();

        foreach (DriveInfo drive in drives)
        {
            // Выводим информацию о диске
            if (drive.IsReady)
            {
                string driveName = drive.Name.Substring(0, drive.Name.Length - 1);
                Debug.Log(driveName);
                if (!isLicensed) {
                    isLicensed = expectedDrive == GetModelFromDrive(driveName);
                }
                Debug.Log(isLicensed);
            }
            else
            {
                Debug.Log($"Диск: {drive.Name} не готов.");
            }
        }
        if (isLicensed == false) {
            Debug.Log("Спиратил сучка");
            Application.Quit();
        }
    }

}