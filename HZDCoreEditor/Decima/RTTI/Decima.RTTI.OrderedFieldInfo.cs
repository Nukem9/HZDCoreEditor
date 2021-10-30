using System;
using System.Linq.Expressions;

namespace Decima
{
    public static partial class RTTI
    {
        /// <summary>
        /// Helper for assigning values to class members when reading binary data. This automatically handles members
        /// defined with multiple inheritance.
        /// </summary>
        public sealed class OrderedFieldInfo
        {
            public sealed class Entry
            {
                public readonly RttiField MIBase;
                public readonly RttiField Field;
                public readonly bool SaveStateOnly;

                public readonly Action<object, object> SetValue;
                public readonly Func<object, object> GetValue;

                public Entry(RttiField miBase, RttiField field, bool saveStateOnly)
                {
                    MIBase = miBase;
                    Field = field;
                    SaveStateOnly = saveStateOnly;

                    SetValue = CreateSetter(MIBase, Field);
                    GetValue = CreateGetter(MIBase, Field);
                }

                private static Action<object, object> CreateSetter(RttiField mi, RttiField member)
                {
                    // void Setter(object classObj, object value)
                    // {
                    //  var a = (typeof(T.Member))value;
                    //  T b;
                    //
                    //  if (MIBase)
                    //    b = ((U)classObj).BaseMember;// Assumes BaseMember is of type T already. No conversion.
                    //  else
                    //    b = (T)classObj;
                    //
                    //  b.Member = a;
                    // }
                    var classObj = Expression.Parameter(typeof(object), "classObj");                        // Parameter "object classObj"
                    var valueObj = Expression.Parameter(typeof(object), "value");                           // Parameter "object value" 

                    // var a = (typeof(T.Member))value;
                    // T b;
                    Expression a = Expression.Convert(valueObj, member.Type);                               // Cast "value" to "typeof(classObj.Member)"
                    Expression b;

                    if (mi != null)
                    {
                        var convertedClassObj = Expression.Convert(classObj, mi.MemberInfo.DeclaringType);  // (U)( classObj )
                        b = Expression.MakeMemberAccess(convertedClassObj, mi.MemberInfo);                  // (U)( classObj ).BaseMember
                    }
                    else
                    {
                        b = Expression.Convert(classObj, member.MemberInfo.DeclaringType);                  // (T)( classObj )
                    }

                    var memberAccess = Expression.MakeMemberAccess(b, member.MemberInfo);                   // Access "b.Member"
                    var assignment = Expression.Assign(memberAccess, a);                                    // b.Member = value
                    var lambda = Expression.Lambda<Action<object, object>>(assignment, classObj, valueObj);

                    return lambda.Compile();
                }

                private static Func<object, object> CreateGetter(RttiField mi, RttiField member)
                {
                    // object Getter(object classObj)
                    // {
                    //  T b;
                    //
                    //  if (MIBase)
                    //    b = ((U)classObj).BaseMember;// Assumes BaseMember is of type T already. No conversion.
                    //  else
                    //    b = (T)classObj;
                    //
                    //  return (object)b.Member;
                    // }
                    var classObj = Expression.Parameter(typeof(object), "classObj");                        // Parameter "object classObj"

                    // T b;
                    Expression b;

                    if (mi != null)
                    {
                        var convertedClassObj = Expression.Convert(classObj, mi.MemberInfo.DeclaringType);  // (U)( classObj )
                        b = Expression.MakeMemberAccess(convertedClassObj, mi.MemberInfo);                  // (U)( classObj ).BaseMember
                    }
                    else
                    {
                        b = Expression.Convert(classObj, member.MemberInfo.DeclaringType);                 // (T)( classObj )
                    }

                    var memberAccess = Expression.MakeMemberAccess(b, member.MemberInfo);                   // Access "b.Member"
                    var convertedMember = Expression.Convert(memberAccess, typeof(object));                 // Cast to object
                    var lambda = Expression.Lambda<Func<object, object>>(convertedMember, classObj);

                    return lambda.Compile();
                }
            }

            public readonly RttiField[] MIBases;
            public readonly Entry[] Members;

            public OrderedFieldInfo(RttiField[] bases, Entry[] members)
            {
                MIBases = bases;
                Members = members;
            }
        }
    }
}
