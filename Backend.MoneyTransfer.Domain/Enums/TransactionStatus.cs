namespace Backend.MoneyTransfer.Domain.Enums;

public enum TransactionStatus
{
    Pending = 0,      // İşlem henüz tamamlanmamış
    Completed = 1,    // İşlem başarıyla tamamlanmış
    Failed = 2,       // İşlem başarısız
    Cancelled = 3,    // İşlem iptal edilmiş
    Reversed = 4,     // İşlem ters çevrilmiş (geri alınmış)
    OnHold = 5        // İşlem beklemede (örneğin manuel onay bekliyor)
}