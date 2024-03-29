﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PRBD_Framework;
using prbd_2021_g01.Properties;
using prbd_2021_g01.Model;

namespace prbd_2021_g01.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TeacherMainView : WindowBase
    {
        public TeacherMainView()
        {
            InitializeComponent();
        }

        private void Vm_OnLogout() { 
            App.NavigateTo<LoginView>(); 
        }

        private void Vm_DisplayCourse(Course course, bool isNew) {
            if (course != null) {
                // tag has to be unique (ipt when we open tabs)
                // convention : class name and its id (with "-" between them)
                var tag = "course-" + course.Id.ToString();
                var tab = tabControl.FindByTag(tag);
                if (tab == null)
                    //tabControl.Add(null, "<new course>");
                    tabControl.Add(
                        new TeacherCourseDetailView(course, isNew),
                        isNew ? "<new course>" : course.Title, 
                        tag
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayQuiz(Quiz quiz, bool isNew)
        {
            if (quiz != null)
            {
                // tag has to be unique (ipt when we open tabs)
                // convention : class name and its id (with "-" between them)
                var tag = "quiz-" + quiz.Id.ToString();
                var tab = tabControl.FindByTag(tag);
                if (tab == null)
                    //tabControl.Add(null, "<new course>");
                    tabControl.Add(
                        new TeacherCourseQuizDetailView(quiz, isNew),
                        isNew ? "<new quiz>" : quiz.Title,
                        tag
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_RenameTab(Course course, string header) {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null) {
                tab.Header = tab.Tag = header = string.IsNullOrEmpty(header) ? "<new course>" : header;
            }
            // TODO: check to add red cross to close the tab
            //tabControl.HasCloseButton = true;
        }

        private void Vm_CloseTab(Course course) {
            var tag = "course-" + course.Id.ToString();
            var tab = tabControl.FindByTag(tag);
            tabControl.Items.Remove(tab);
        }

        private void WindowBase_KeyDown(object sender, KeyEventArgs e) { 
            if (e.Key == Key.Escape) Close(); 
        }
        private void Language_Click(object sender, RoutedEventArgs e) { 
            var lang = (sender as MenuItem).CommandParameter?.ToString(); 
            App.ChangeCulture(lang); 
            Settings.Default.Culture = lang; 
            Settings.Default.Save(); 
        }

        private void Menu_Profile_Click(object sender, RoutedEventArgs e) {
            var tag = "Profile";
            var tab = tabControl.FindByTag(tag);
            if (tab == null)
                tabControl.Add(new ProfileView(), Properties.Resources.Menu_Profile, tag);
            else
                tabControl.SetFocus(tab);
        }


    }
}
