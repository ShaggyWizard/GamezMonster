using UnityEngine;


public interface ITrajectory
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Velocity { get; }

    public void SetPosition(Vector3 newPosition);
    public void SetRotation(Quaternion newRotation);
    public void SetDirection(Vector3 newDirection);
    public void SetSpeed(float newSpeed);
}