namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: Represent elements/models that can be checked (E.g. in Ui)
/// Created by: tseb
/// </summary>
public interface ICheckable
{
    bool IsChecked { get; set; }
}
