#pragma once

using namespace System;

namespace Microsoft { namespace HandsFree { namespace Sensors { namespace Interop { namespace EyeTech {

ref class EyeTechApiException :
	public Exception
{
private:
	const QLError _error;
public:
	EyeTechApiException(QLError error) : _error(error)
	{

	}

	static void ThrowIfError(QLError error)
	{
		if (error != QL_ERROR_OK)
		{
			auto ex = gcnew EyeTechApiException(error);
			throw ex;
		}
	}
};

} } } } }


