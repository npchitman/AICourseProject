namespace Algorithms{
  public static AiTracking{
    /// <summary>
    /// 拦截追逐算法，可以预测追逐目标的移动位置进行追踪
    /// </summary>
    Vector2 AI_PredictionPursuit()
    {
      //首先计算两者的距离
      Vector2 ToPursuit = m_pursuitTarget.Position - m_pursuiter.Position;

      //局部坐标的前进方向向量的点积
      float RelativeHeading = DotProduct(m_pursuiter.vHeading, m_pursuitTarget.vHeading);

      //如果两者的距离在追逐者的局部向量前进方向的投影大于零，那么追逐者应该直接向被追踪对象移动，这里的20是被追踪对象的反方向和追踪者的朝向在18度(cos(18)=0.95)就被认为是面对着的
      if(DotProduct(ToPursuit,m_pursuiter.vHeading)>0&&RelativeHeading<-0.95f)
      {
        // Debug.Log("relativeHeading:" + RelativeHeading);
        return AI_Seek(m_pursuitTarget.Position);
      }

      //预测被追踪者的位置，预测的时间正比于被追踪者与追踪者的距离，反比与追踪者的速度和当前靠近被追踪者的被预测位置(好绕口啊,慢慢理解吧!)
      float toPursuitLenght = Mathf.Sqrt(ToPursuit.x*ToPursuit.x+ToPursuit.y*ToPursuit.y);
      float LookAheadTime = toPursuitLenght / (m_pursuiter.MaxSpeed + m_pursuitTarget.Speed);

      //预测的位置,其实这个位置会一直在被追踪对象的局部坐标前方
      Vector2 predictionPos=m_pursuitTarget.Position+m_pursuitTarget.Velocity*LookAheadTime;
      Debug.Log("preditionx:" + predictionPos.x + "preditiony:" + predictionPos.y);
      AimPredictionPos.transform.position = new Vector3(predictionPos.x, predictionPos.y, 0);
      return AI_Seek(predictionPos);
    }

    /// <summary>
    /// 到达指定位置
    /// </summary>
    /// <param name="TargetPos">指定位置向量</param>
    /// <returns></returns>
    Vector2 AI_Seek(Vector2 TargetPos)
    {
      //计算目标位置与追踪对象位置的向量并且将其归一化
      Vector2 DesiredVelocity = (TargetPos - m_pursuiter.Position).normalized*m_pursuiter.MaxSpeed;
      //直接相减就可以得到一个中间的过渡向量，避免直接生硬的改变
      DesiredVelocity = DesiredVelocity - m_pursuiter.Velocity;
      return DesiredVelocity;
    }

    /// <summary>
    /// 计算矩阵A与矩阵B的点积
    /// </summary>
    float DotProduct(Vector2 A, Vector2 B) => return A.x * B.x + A.y * B.y;
  }
}
