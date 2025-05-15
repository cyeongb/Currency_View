using System;
using System.Collections.Generic;

namespace CurrencyExchangeApp.Models
{
    public class Currency
    {
        public string Code { get; set; }               // 통화 코드 (USD, EUR 등)
        public string Name { get; set; }               // 통화 이름
        public double Rate { get; set; }               // 환율 (기준 통화 대비)
        public double PreviousRate { get; set; }       // 이전 환율 (변동률 계산용)
        public double ChangePercentage { get; set; }   // 변동률 (%)
        public DateTime LastUpdate { get; set; }       // 마지막 업데이트 시간

        // 변동률 계산
        public void CalculateChangePercentage()
        {
            if (PreviousRate > 0 && Math.Abs(PreviousRate - Rate) > 0.000001) // 부동소수점 비교를 위한 epsilon 사용
            {
                ChangePercentage = ((Rate - PreviousRate) / PreviousRate) * 100;
            }
            else
            {
                ChangePercentage = 0;
            }
        }

        // 통화별 디스플레이 이름 반환
        public string DisplayName => $"{Code} - {Name}";

        // 변동률 방향 확인 속성
        public bool IsIncreasing => ChangePercentage > 0.01;
        public bool IsDecreasing => ChangePercentage < -0.01;
        public bool IsUnchanged => Math.Abs(ChangePercentage) <= 0.01;
    }
}