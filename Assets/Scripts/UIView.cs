using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIView : MonoBehaviour
{
    public Color color = Color.clear;

    private List<Vector3> newVertices = new List<Vector3>();

    private List<int> newTriangles = new List<int>();

    private Mesh mesh;

    private float ScreenHeight;

    private float ScreenWidth;


    // Use this for initialization
    void Start()
    {
        this.ScreenHeight = 2f * Camera.main.orthographicSize;
        this.ScreenWidth = this.ScreenHeight * Camera.main.aspect;
        this.mesh = base.GetComponent<MeshFilter>().mesh;
        float z = 0f;
        this.newVertices.Add(new Vector3(0f, 0f, z));
        this.newVertices.Add(new Vector3(this.ScreenWidth, 0f, z));
        this.newVertices.Add(new Vector3(this.ScreenWidth, this.ScreenHeight, z));
        this.newVertices.Add(new Vector3(0f, this.ScreenHeight, z));
        this.newTriangles.Add(0);
        this.newTriangles.Add(1);
        this.newTriangles.Add(3);
        this.newTriangles.Add(1);
        this.newTriangles.Add(2);
        this.newTriangles.Add(3);
        this.mesh.Clear();
        this.mesh.vertices = this.newVertices.ToArray();
        this.mesh.triangles = this.newTriangles.ToArray();
        this.mesh.RecalculateNormals();
        if (this.color == Color.clear)
        {
            this.setColor(Color.clear);
        }
        else
        {
            this.setColor(this.color);
        }
    }

    //change color
    public void setColor(Color color)
    {
        this.color = color;
        MeshRenderer component = base.GetComponent<MeshRenderer>();
        component.material.color = this.color;
    }

    public void setAlpha(float alpha)
    {
        MeshRenderer component = base.GetComponent<MeshRenderer>();
        component.material.color = new Color(this.color.r, this.color.g, this.color.b, alpha);
    }

    public void setPosition(float x, float y, float z = 0f)
    {
        base.transform.position = new Vector3(x, y, z);
    }

    public void setCenter(float x, float y, float z = 0f)
    {
        float num = 2f * Camera.main.orthographicSize;
        float num2 = num * Camera.main.aspect;
        base.transform.position = new Vector3(x - num2 * base.transform.localScale.x / 2f, y - num * base.transform.localScale.y / 2f, z);
    }

    public void setSize(float width, float height)
    {
        float num = 2f * Camera.main.orthographicSize;
        float num2 = num * Camera.main.aspect;
        base.transform.localScale = new Vector3(width / num2, height / num, 1f);
    }

    public void setSquareSize(float width)
    {
        float num = 2f * Camera.main.orthographicSize;
        float num2 = num * Camera.main.aspect;
        base.transform.localScale = new Vector3(width / num2, width / num, 1f);
    }

    public void setProportionalSize(float width, float height)
    {
        float num = 2f * Camera.main.orthographicSize;
        float num2 = num * Camera.main.aspect;
        base.transform.localScale = new Vector3(width * num2 / num2, height * num / num, 1f);
    }

    public void setProportionalSquareSize(float width)
    {
        float num = 2f * Camera.main.orthographicSize;
        float num2 = num * Camera.main.aspect;
        base.transform.localScale = new Vector3(width * num2 / num2, width * num2 / num, 1f);
    }
}
