////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  QL2Utility.h
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QL2_UTILITY_H
#define	QL2_UTILITY_H

#include <QuickLink2.h>
#include <Windows.h>


// To link implicitly comment out the next line. If implicit linking is done then QuickLink2.lib
// must be linked as an additional dependency. 
#define EXPLICIT_LINKING

class QL2Utility
{
public:
	// Constructor
	QL2Utility();

	// Destructor
	~QL2Utility();

	// Load the Quick Link 2 dll into memory.
	bool Load(const TCHAR* libraryPath = 0);

	// Unload the Quick Link 2 dll from memory.
	void Unload();

	// Query if the library has been loaded.
	bool IsLoaded();
};

#endif