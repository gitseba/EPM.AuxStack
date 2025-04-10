namespace LogProcessor.Application.State;

/// <summary>
/// Purpose: This will serve as a key for every action that needs to be executed on a state <see cref="FilesStateManager"/>
/// Created by: tseb
/// </summary>
public enum FilesStateEnum
{
    RootPathSelected, // source directory path where files exist
    FilePathSelected, // path of the selected file
    FileMetadata      // metadata details about content
}
