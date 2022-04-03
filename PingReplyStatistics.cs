using System.Net.NetworkInformation;
using System.Net;

public class PingReplyStatistics
{
    #region Field && Properties
    public List<PingReply> Replies { get; private set; }
    public IPAddress IPAddress { get; private set; }
    public long TotalRoundtripTime { get; private set; }
    public long MinimumRoundtripTime { get; private set; }
    public long AvarageRoundtripTime
    {
        get
        {
            if (TotalSuccessfulPings > 0)
                return TotalRoundtripTime / TotalSuccessfulPings;
            return 0;
        }
    }
    public long MaximumRoundtripTime { get; private set; }
    public int TotalSent { get { return Replies.Count; } }
    public int TotalSuccessfulPings { get; private set; }
    public int TotalFailedPings { get { return TotalSent - TotalSuccessfulPings; } }
    #endregion
    public PingReplyStatistics() : this(null) { }
    public PingReplyStatistics(List<PingReply> replies)
    {
        Initialize(replies);
    }

    public void Initialize(List<PingReply> replies)
    {
        Replies = new List<PingReply>();
        foreach (var reply in replies)
            Add(reply);
    }
    public void Add(PingReply reply)
    {
        if (!Replies.Contains(reply))
        {
            Replies.Add(reply);
            IPAddress = reply.Address;
            if (reply.RoundtripTime > 0)
            {
                if (MinimumRoundtripTime == 0 || reply.RoundtripTime < MinimumRoundtripTime)
                    MinimumRoundtripTime = reply.RoundtripTime;
                if (reply.RoundtripTime > MaximumRoundtripTime)
                    MaximumRoundtripTime = reply.RoundtripTime;
                TotalRoundtripTime += reply.RoundtripTime;
            }
            //
            if (reply.Status == IPStatus.Success)
                TotalSuccessfulPings++;
        }
        else
        {
            throw new ArgumentException("reply allready exists in the stats");
        }
    }

    public override string ToString()
    {
        return $"IPAddress:{IPAddress},Successfull:{TotalSuccessfulPings},Failed:{TotalFailedPings},TotalRoundtripTime:{TotalRoundtripTime}," +
               $",Minimum:{MinimumRoundtripTime},Avarage:{AvarageRoundtripTime},Maximum:{MaximumRoundtripTime}";
    }
}
