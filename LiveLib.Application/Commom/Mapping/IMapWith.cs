using System;
using System.Runtime.CompilerServices;
using AutoMapper;

namespace LiveLib.Application.Common.Mapping
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var sourceType = typeof(T);
            var destinationType = GetType();

            profile.CreateMap(sourceType, destinationType)
                .ReverseMap()
                .ForAllMembers(opts =>
                {
                    opts.Condition((src, dest, srcMember, destMember) =>
                    {
                        if (srcMember == null)
                        {
                            return false;
                        }

                        var memberType = srcMember.GetType();

                        if (memberType.IsValueType && !memberType.IsEnum)
                        {
                            object defaultValue;

                            try
                            {
                                defaultValue = RuntimeHelpers.GetUninitializedObject(memberType);
                            }
                            catch (TypeInitializationException)
                            {
                                return true;
                            }

                            return !srcMember.Equals(defaultValue);
                        }

                        return true;
                    });
                });
        }
    }
}