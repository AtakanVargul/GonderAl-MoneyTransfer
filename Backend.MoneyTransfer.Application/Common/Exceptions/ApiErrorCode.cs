namespace Backend.MoneyTransfer.Application.Common.Exceptions;

/// <summary>
/// Generic Error Codes
/// </summary>
public class ApiErrorCode
{
    public const string InternalError = "100";
    public const string ServiceForbidden = "101";
    public const string InvalidParameters = "102";
    public const string NotFound = "103";
    public const string ValidationError = "104";
    public const string InvalidCredentials = "105";
    public const string InvalidPermissions = "106";
    public const string DuplicateRecord = "107";
    public const string AlreadyInUse = "108";
    public const string InvalidAuthorizationHeader = "109";
    public const string AuthorizationSignatureMismatch = "110";
    public const string AuthorizationTimestampExpired = "111";
    public const string AuditableMissingInfo = "112";
    public const string UserInBlacklist = "113";
    public const string InvalidNewPassword = "301";
    public const string InvalidOtp = "302";
    public const string InvalidRegister = "303";
    public const string InvalidNewEmail = "304";
    public const string InvalidKycLevel = "305";
    public const string LockedOut = "306";
    public const string LoginFailed = "307";
    public const string PasswordExpired = "308";
    public const string PasswordsNotMatched = "309";
    public const string PasswordHistoryRequirement = "310";
    public const string PasswordContent = "311";
    public const string PasswordLength = "312";
    public const string PasswordRepetitiveCharacter = "313";
    public const string PasswordSuccessiveCharacter = "314";
    public const string HasRelationalEntitySubkey = "315";
    public const string ForbiddenAgreementDocOperation = "316";
    public const string BirthdateOutOfRange = "317";
    public const string AlreadyDeactivated = "318";
    public const string DuplicateField = "319";
    public const string TheSameAsOldPassword = "320";
    public const string InsufficientBalance = "321";
}