
using System;

public class IronSourceError
{
	private string description;
	private int code;

	public int getErrorCode ()
	{
		return code;
	}

	public string getDescription ()
	{
		return description;
	}

	public int getCode ()
	{
		return code;
	}

	public IronSourceError (int errorCode, string errorDescription)
	{
		code = errorCode;
		description = errorDescription;
	}

	public override string ToString ()
	{
		return code + " : " + description;
	}
}

