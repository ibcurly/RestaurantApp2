package com.example.dibby.restaurantapp2;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class LoadDataActivity extends AppCompatActivity {

    String serverURL;
    String JSONMenu;

    String[][] foodItems;
    String[][] drinkItems;
    boolean isDataLoaded = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_load_data);
    }

    @Override
    public void onBackPressed() {
        Log.d("DEBUG: ", "Back button pressed, saving food and drink arrays");
        Bundle bundle = new Bundle();
        bundle.putString("serverURL", serverURL);
        bundle.putBoolean("isDataLoaded", isDataLoaded);
        bundle.putSerializable("foodItems", foodItems);
        bundle.putSerializable("drinkItems", drinkItems);
        Intent intent = new Intent(this, LoadDataActivity.class);
        intent.putExtras(bundle);
        setResult(RESULT_OK, intent);
        finish();
    }

    public void downloadDataButton(View view) {
        EditText serverURLValue = (EditText)findViewById(R.id.serverURLValue);
        serverURL = serverURLValue.getText().toString();
        if(serverURL.equals(""))
            serverURL = "http://danhpham312.com";

        getDrinks();
        getFood();
    }

    //This is called once the JSON for the drinks is received.
    public void onDrinkResopnse(){
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONMenu).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray jsonDrinkArray = jsonRootObject.optJSONArray("drinks");

            // Initialize the food and drink arrays based on length
            drinkItems = new String[jsonDrinkArray.length()][3];
            Log.d("DEBUG:", "Drink array items: " + jsonDrinkArray.length());

            //Iterate the jsonArray and populate the food and drink arrays from the info of JSONObjects
            for (int i = 0; i < jsonDrinkArray.length(); i++) {
                JSONObject jsonObject = jsonDrinkArray.getJSONObject(i);
                drinkItems[i][0] = jsonObject.optString("ID");
                drinkItems[i][1] = jsonObject.optString("ITEM");
                drinkItems[i][2] = jsonObject.optString("PRICE");
            }

        } catch (JSONException e) {
            e.printStackTrace();
        }
        isDataLoaded = true;
    }

    // This is called when the JSON for the food is received.
    public void onFoodResponse(){
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONMenu).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray jsonFoodArray = jsonRootObject.optJSONArray("foods");

            // Initialize the food and drink arrays based on length
            foodItems = new String[jsonFoodArray.length()][3];
            Log.d("DEBUG:", "Food array items: " + jsonFoodArray.length());

            //Iterate the jsonArray and populate the food and drink arrays from the info of JSONObjects
            for (int i = 0; i < jsonFoodArray.length(); i++) {
                JSONObject jsonObject = jsonFoodArray.getJSONObject(i);
                foodItems[i][0] = jsonObject.optString("ID");
                foodItems[i][1] = jsonObject.optString("ITEM");
                foodItems[i][2] = jsonObject.optString("PRICE");
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        isDataLoaded = true;
    }

    // This is the call to server to get the JSON for drinks
    public void getDrinks(){
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = serverURL + "/dmenu.php";

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET,url,
                null, new Response.Listener<JSONObject>() {

            @Override
            public void onResponse(JSONObject response) {
                Log.d("DEBUG", response.toString());
                JSONMenu = response.toString();
                onDrinkResopnse();
                Toast.makeText(getApplicationContext(), "Drinks loaded.", Toast.LENGTH_SHORT).show();
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                VolleyLog.d("DEBUG", "Error: " + error.getMessage());
                Log.e("EXCEPTPTION", "Site Info Error: " + error.getMessage());
                Toast.makeText(getApplicationContext(), "Data load failure - check URL", Toast.LENGTH_LONG).show();
            }
        });

        queue.add(req);
    }

    // This is the call to server to get the JSON for food
    public void getFood(){
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = serverURL + "/fmenu.php";

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET,url,
                null, new Response.Listener<JSONObject>() {

            @Override
            public void onResponse(JSONObject response) {
                Log.d("DEBUG", response.toString());
                JSONMenu = response.toString();
                onFoodResponse();
                Toast.makeText(getApplicationContext(), "Food loaded.", Toast.LENGTH_SHORT).show();
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                VolleyLog.d("DEBUG", "Error: " + error.getMessage());
                Log.e("EXCEPTPTION", "Site Info Error: " + error.getMessage());
                Toast.makeText(getApplicationContext(), "Data load failure - check URL", Toast.LENGTH_LONG).show();
            }
        });
        queue.add(req);
    }

}