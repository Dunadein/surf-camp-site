using System.Runtime.Serialization;

namespace YandexPaymentProvider.Response
{
    internal enum ResponseStatus
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "refused")]
        Refused,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "ext_auth_required")]
        ExtAuthRequired
    }
}
