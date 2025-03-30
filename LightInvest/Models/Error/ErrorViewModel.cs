namespace LightInvest.Models.Error
{
	/// <summary>
	/// Represents an error view model used for displaying error information in the UI.
	/// </summary>
	public class ErrorViewModel
	{
		/// <summary>
		/// Gets or sets the request ID associated with the error.
		/// </summary>
		/// <value>
		/// A string representing the unique identifier for the request.
		/// </value>
		public string? RequestId { get; set; }

		/// <summary>
		/// Gets a value indicating whether the request ID should be shown in the error view.
		/// </summary>
		/// <value>
		/// Returns <c>true</c> if the request ID is not null or empty; otherwise, <c>false</c>.
		/// </value>
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
