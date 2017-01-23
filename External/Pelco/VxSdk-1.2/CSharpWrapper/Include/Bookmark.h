// Declares the bookmark class.
#ifndef Bookmark_h__
#define Bookmark_h__

#include "VxSdk.h"
#include "DataSource.h"
#include "Utils.h"

namespace CPPCli {

    /// <summary>
    /// The Bookmark class represents a point in time of interest with reference to a particular data source.
    /// </summary>
    public ref class Bookmark {
    public:

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="vxBookmark">The vx bookmark.</param>
        Bookmark(VxSdk::IVxBookmark* vxBookmark);

        /// <summary>
        /// Destructor.
        /// </summary>
        virtual ~Bookmark() {
            this->!Bookmark();
        }

        /// <summary>
        /// Finaliser.
        /// </summary>
        !Bookmark();

        /// <summary>
        /// Refreshes this instances properties.
        /// </summary>
        /// <returns>The <see cref="Results::Value">Result</see> of updating the properties.</returns>
        Results::Value Refresh();

        /// <summary>
        /// Gets or sets the friendly description of the bookmark.
        /// </summary>
        /// <value>The friendly description.</value>
        property System::String^ Description {
        public:
            System::String^ get() { return gcnew System::String(_bookmark->description); }
            void set(System::String^ value) {
                char description[64];
				VxSdk::Utilities::StrCopySafe(description, Utils::ConvertSysStringNonConst(value));
                _bookmark->SetDescription(description);
            }
        }

        /// <summary>
        /// Gets the unique bookmark identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        property System::String^ Id {
        public:
            System::String^ get() { return gcnew System::String(_bookmark->id); }
        }

        /// <summary>
        /// Gets the date and time at which the point of interest occurred.
        /// </summary>
        /// <value>A default <c>DateTime</c> if it fails, else the <c>DateTime</c> at which the point of interest occurred.</value>
        property System::DateTime Time {
        public:
            System::DateTime get() { return Utils::ConvertCharToDateTime(_bookmark->time); }
        }

        /// <summary>
        /// Gets the data source associated with this bookmark.
        /// </summary>
        /// <value>The associated data source.</value>
        property CPPCli::DataSource^ DataSource {
        public:
            CPPCli::DataSource^ get() { return _GetDataSource(); }
        }

    internal:
        VxSdk::IVxBookmark* _bookmark;
        CPPCli::DataSource^ _GetDataSource();
    };
}
#endif // Bookmark_h__
